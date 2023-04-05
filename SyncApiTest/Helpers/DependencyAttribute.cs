namespace SyncApiTest.Helpers;

[AttributeUsage(AttributeTargets.Property)]
public class DependencyAttribute : Attribute
{
    public string PropertyName { get; }

    public DependencyAttribute(string propertyName)
    {
        PropertyName = propertyName;
    }
}
