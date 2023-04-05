using System.Runtime.CompilerServices;

namespace SyncApiTest.Helpers;

public class PropertyDependencySorter : IPropertyDependencySorter
{
    public List<PropertyInfo> SortByDependency(IEnumerable<PropertyInfo> properties)
    {
        try
        {
            
        var propertyGraph = new Dictionary<PropertyInfo, List<PropertyInfo>>();
        var propertySet = new HashSet<PropertyInfo>(properties);

        foreach (var property in propertySet)
        {
            var dependentProperties = property.GetCustomAttributes(typeof(DependencyAttribute), true)
                .Cast<DependencyAttribute>()
                .Select(attr => attr.PropertyName)
                .Select(name => property.DeclaringType.GetProperty(name))
                .Where(p => p != null && propertySet.Contains(p))
                .ToList();

            propertyGraph[property] = dependentProperties;
        }

        var sortedProperties = new List<PropertyInfo>();
        var visitedProperties = new HashSet<PropertyInfo>();

        foreach (var property in propertySet)
        {
            if (!visitedProperties.Contains(property))
            {
                Visit(property, propertyGraph, sortedProperties, visitedProperties);
            }
        }

        return sortedProperties;
        
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static void Visit(
        PropertyInfo property,
        Dictionary<PropertyInfo, List<PropertyInfo>> propertyGraph,
        List<PropertyInfo> sortedProperties,
        HashSet<PropertyInfo> visitedProperties)
    {
        visitedProperties.Add(property);

        foreach (var dependentProperty in propertyGraph[property])
        {
            if (!visitedProperties.Contains(dependentProperty))
            {
                Visit(dependentProperty, propertyGraph, sortedProperties, visitedProperties);
            }
        }

        sortedProperties.Add(property);
    }
}

public interface IPropertyDependencySorter
{
    List<PropertyInfo> SortByDependency(IEnumerable<PropertyInfo> properties);
}