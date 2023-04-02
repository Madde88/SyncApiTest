namespace SyncApiTest.Services;

public class SyncService : ISyncService
{
    private readonly ApplicationDbContext _dbContext;

    public SyncService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
    }

    private async Task<List<TEntity>> SyncData<TEntity>(IEnumerable<TEntity> clientData) where TEntity : BaseEntity
    {
        try
        {

            // Get server data
            var serverData = await _dbContext.Set<TEntity>().ToListAsync();

            // Create dictionaries of client and server data for efficient lookups
            var clientDict = clientData.ToDictionary(e => e.Id);
            var serverDict = serverData.ToDictionary(e => e.Id);

            // Iterate through client data and update server data
            foreach (var clientEntity in clientData)
            {
                // Check if entity exists on server
                if (serverDict.TryGetValue(clientEntity.Id, out var serverEntity))
                {
                    // Update server entity if client version is newer
                    if (clientEntity.LocalDateUpdate > serverEntity.ServerDateUpdated)
                    {
                        _dbContext.Entry(clientEntity).State = EntityState.Modified;
                        //Generic way to map data
                        serverEntity.ServerDateUpdated = DateTime.UtcNow;
                        serverEntity.LastSyncedAt = DateTime.UtcNow;
                    }

                    // Check if client entity is marked as deleted
                    if (clientEntity.Deleted == true)
                    {
                        // Mark server entity as deleted
                        serverEntity.Deleted = true;
                        serverEntity.ServerDateUpdated = DateTime.UtcNow;
                    }
                }
                // Otherwise, add client entity to server database
                else
                {
                    _dbContext.Set<TEntity>().Add(clientEntity);
                    _dbContext.Entry(clientEntity).State = EntityState.Added; 
                }
            }

            // Save changes to server database
            await _dbContext.SaveChangesAsync();

            // Return updated server data to client
            return serverData;

        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<SyncPayload> HandleSync(SyncInput input)
    {
        try
        {
            await _dbContext.Database.BeginTransactionAsync();
       
            var result = new Dictionary<string, object>();
            var inputContent = input.GetType().GetProperties();

            foreach (var entity in inputContent)
            {
                var entityType = entity.PropertyType.GetGenericArguments().FirstOrDefault();
                if (entityType != null)
                {
                    var entities = entity.GetValue(input) as IEnumerable;

                    var syncDataMethod = typeof(SyncService)
                        .GetMethod("SyncData", BindingFlags.Instance | BindingFlags.NonPublic);
                    var methodInfo = syncDataMethod?.MakeGenericMethod(entityType);
                
                    var syncedEntities = await (dynamic)methodInfo.Invoke(this, new object[] { entities });
                    result.Add(entityType.Name, ((IEnumerable)syncedEntities).Cast<object>().ToList());
                }
            }
        
            // Create an instance of SyncPayload using reflection
            var syncPayload = Activator.CreateInstance(typeof(SyncPayload), new object[] { }) as SyncPayload;

            // Loop through the dictionary and set the properties of the SyncPayload object using reflection
            foreach (var kvp in result)
            {
                var propName = kvp.Key;
                var prop = syncPayload.GetType().GetProperty(propName);
                var propValue = kvp.Value;

                // Convert the List<object> to List<T> using reflection
                var propListType = typeof(List<>).MakeGenericType(prop.PropertyType.GenericTypeArguments[0]);
                var propList = Activator.CreateInstance(propListType, new object[] { propValue }) as IList;
                prop.SetValue(syncPayload, propList);
            }

            await _dbContext.Database.CommitTransactionAsync();
            // Return the SyncPayload object
            return syncPayload;
        }
        catch (Exception e)
        {
            await _dbContext.Database.RollbackTransactionAsync();
            Console.WriteLine(e);
            throw;
        }
    }
}

public interface ISyncService
{
    Task<SyncPayload> HandleSync(SyncInput input);
}