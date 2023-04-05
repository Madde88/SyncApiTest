namespace SyncApiTest.Services;
public class SortingEntities : ISortingEntities
    {
        public IEnumerable<BaseEntity> SortEntitiesTopologically(SyncInput input)
        {
            var graph = new Dictionary<BaseEntity, List<BaseEntity>>();

            // Add entities to graph
            foreach (var entity in GetAllEntities(input))
            {
                AddEntityToGraph(entity, graph);
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

        private IEnumerable<BaseEntity> GetAllEntities(SyncInput input)
        {
            var inputType = input.GetType();
            var entityProperties = inputType.GetProperties()
                .Where(p => typeof(IEnumerable<BaseEntity>).IsAssignableFrom(p.PropertyType));

            foreach (var property in entityProperties)
            {
                var entities = (IEnumerable<BaseEntity>)property.GetValue(input);
                if (entities != null)
                {
                    foreach (var entity in entities)
                    {
                        yield return entity;
                    }
                }
            }
        }

        private void AddEntityToGraph(BaseEntity entity, Dictionary<BaseEntity, List<BaseEntity>> graph)
        {
            if (!graph.ContainsKey(entity))
            {
                graph[entity] = new List<BaseEntity>();
            }

            var dependencies = entity.GetDependencies();
            foreach (var dependency in dependencies)
            {
                if (dependency != null)
                {
                    if (!graph.ContainsKey(dependency))
                    {
                        graph[dependency] = new List<BaseEntity>();
                    }

                    graph[dependency].Add(entity);
                }
            }
        }

        private void TopologicalSort(BaseEntity entity, HashSet<BaseEntity> visited, List<BaseEntity> sortedEntities, Dictionary<BaseEntity, List<BaseEntity>> graph)
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
        IEnumerable<BaseEntity> SortEntitiesTopologically(SyncInput input);
    }