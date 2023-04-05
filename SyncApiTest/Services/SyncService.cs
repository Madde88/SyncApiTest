namespace SyncApiTest.Services;

public class SyncService : ISyncService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPropertyDependencySorter _sortingEntities;
    private readonly MapperlyMapper _mapper;

    public SyncService(IDbContextFactory<ApplicationDbContext> dbContextFactory, IPropertyDependencySorter sortingEntities)
    {
        _sortingEntities = sortingEntities;
        _dbContext = dbContextFactory.CreateDbContext();
        _mapper = new MapperlyMapper();
    }

    private async Task<List<TEntity>> SyncData<TEntity>(IEnumerable<TEntity> clientData) where TEntity : BaseEntity
    {
        try
        {
            // Get server data
            var serverData = await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();

            // Create dictionaries of client and server data for efficient lookups
            var clientDict = clientData.ToDictionary(e => e.Id);
            var serverDict = serverData.ToDictionary(e => e.Id);

            // Iterate through client data and update server data
            foreach (var clientEntity in clientData)
            {
                // Check if entity exists on server
                if (serverDict.TryGetValue(clientEntity.Id, out var serverEntity))
                {
                    var _dateCreated = serverEntity.DateCreated;
                    var _localDateUpdated = serverEntity.LocalDateUpdate;
                    var _deleted = serverEntity.Deleted;
                    
                    // Update server entity if client version is newer
                    if (clientEntity.LocalDateUpdate > serverEntity.ServerDateUpdated)
                    {
                        //TODO:Generic way to map data
                        serverEntity = clientEntity;
                        serverEntity.ServerDateUpdated = DateTime.UtcNow;
                        serverEntity.LastSyncedAt = DateTime.UtcNow;
                        serverEntity.DateCreated = _dateCreated;
                        serverEntity.LocalDateUpdate = _localDateUpdated;
                        serverEntity.Deleted = _deleted;
                        _dbContext.Update(serverEntity);
                        serverDict[serverEntity.Id] = serverEntity;
                    }

                    // Check if client entity is marked as deleted
                    if (clientEntity.Deleted == true)
                    {
                        // Mark server entity as deleted
                        serverEntity.Deleted = true;
                        serverEntity.ServerDateUpdated = DateTime.UtcNow;
                        serverEntity.LastSyncedAt = DateTime.UtcNow;
                        _dbContext.Update(serverEntity);
                        serverDict[serverEntity.Id] = serverEntity;
                    }
                }
                // Otherwise, add client entity to server database
                else
                {
                    _dbContext.Set<TEntity>().Add(clientEntity);
                    serverDict.Add(clientEntity.Id, clientEntity);
                }
            }

            // Save changes to server database
            await _dbContext.SaveChangesAsync();

            // Return updated server data to client
            return serverDict.Values.ToList();

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
            var properties = input.GetType().GetProperties();
            var sortedInput = _sortingEntities.SortByDependency(properties);
            await _dbContext.Database.BeginTransactionAsync();
       
            var result = new Dictionary<string, object>();
            //var inputContent = input.GetType().GetProperties();

            foreach (var entity in sortedInput)
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
                var prop = syncPayload.GetType().GetProperty($"{propName}s");
                var propValue = kvp.Value;

                // Convert the List<object> to List<T> using reflection
                var propListType = typeof(List<>).MakeGenericType(prop.PropertyType.GenericTypeArguments[0]);
                
                var propList = Activator.CreateInstance(propListType) as IList;
                foreach (var item in (IEnumerable)propValue)
                {
                    propList?.Add(item);
                }
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