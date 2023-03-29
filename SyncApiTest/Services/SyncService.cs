namespace SyncApiTest.Services;

public class SyncService : ISyncService
{
    private readonly ApplicationDbContext _dbContext;

    public SyncService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private async Task<List<T>> SyncData<T>(List<T> clientData) where T : BaseEntity
    {
        try
        {
            // Get server data
        var serverData = await _dbContext.Set<T>().ToListAsync();

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
                }
            }
            // Otherwise, add client entity to server database
            else
            {
                _dbContext.Set<T>().Add(clientEntity);
            }
        }

        // Iterate through server data and delete entities that no longer exist on client
        foreach (var serverEntity in serverData)
        {
            if (!clientDict.ContainsKey(serverEntity.Id))
            {
                serverEntity.Deleted = true;
                serverEntity.ServerDateUpdated = DateTime.UtcNow;
            }
        }

        // Save changes to server database
        await _dbContext.SaveChangesAsync();

        // Return updated server data to client
        return serverData;
        
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<SyncPayload> HandleSync(SyncInput input)
    {
        var result = new Dictionary<string, object>();
        var inputContent = input.GetType().GetProperties();
        foreach (var entity in inputContent)
        {
            var entityType = entity.PropertyType.GetGenericArguments().FirstOrDefault();
            if (entityType != null)
            {
                var entities = (IEnumerable)entity.GetValue(input);
                var syncedEntities = await SyncData(entities.Cast<BaseEntity>().ToList());
                result.Add(entityType.Name, syncedEntities);
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

        // Return the SyncPayload object
        return syncPayload;
    }

}

public interface ISyncService
{
    Task<SyncPayload> HandleSync(SyncInput input);
}