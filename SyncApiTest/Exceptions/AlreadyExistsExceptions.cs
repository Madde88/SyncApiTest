namespace TestGraphQL.Exceptions;

public class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException(string entity)
        : base($"The {entity} already exist.")
    {
    }
}