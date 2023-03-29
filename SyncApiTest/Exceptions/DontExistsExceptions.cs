namespace TestGraphQL.Exceptions;

public class EntityDontExistsException : Exception
{
    public EntityDontExistsException(string entity)
        : base($"The {entity} doesn't exist.")
    {
    }
}