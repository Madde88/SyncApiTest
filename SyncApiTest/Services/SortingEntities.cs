namespace SyncApiTest.Services;

public class SortingEntities : ISortingEntities
{
    public IEnumerable<BaseEntity> SortEntitiesTopologically<TEntity>(IEnumerable<TEntity> clientData) where TEntity : BaseEntity
    {
        try
        {
            
            // Create a graph of entities and their dependencies
            var graph = new Dictionary<BaseEntity, List<BaseEntity>>();
            foreach (var clientEntity in clientData)
            {
                if (!graph.ContainsKey(clientEntity))
                {
                    graph[clientEntity] = new List<BaseEntity>();
                }

                var dependencies = clientEntity.GetDependencies();
                foreach (var dependency in dependencies)
                {
                    if (dependency != null) // add null check here
                    {
                        if (!graph.ContainsKey(dependency))
                        {
                            graph[dependency] = new List<BaseEntity>();
                        }

                        graph[dependency].Add(clientEntity);
                    }
                }
            }

            // Sort the entities using topological sort
            var sortedEntities = new List<BaseEntity>();
            var visited = new HashSet<BaseEntity>();
            foreach (var entity in graph.Keys)
            {
                TopologicalSort(entity, visited, sortedEntities, graph);
            }
            sortedEntities.Reverse();
            return sortedEntities;
        
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    
    private void TopologicalSort<TEntity>(TEntity entity, HashSet<TEntity> visited, List<TEntity> sortedEntities, Dictionary<TEntity, List<TEntity>> graph) where TEntity : BaseEntity
    {
        if (visited.Contains(entity))
        {
            return;
        }

        visited.Add(entity);

        foreach (var dependency in graph[entity])
        {
            TopologicalSort(dependency, visited, sortedEntities, graph);
        }

        sortedEntities.Add(entity);
    }

}

public interface ISortingEntities
{
    IEnumerable<BaseEntity> SortEntitiesTopologically<TEntity>(IEnumerable<TEntity> clientData)
        where TEntity : BaseEntity;
}