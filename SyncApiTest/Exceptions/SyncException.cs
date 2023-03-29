namespace TestGraphQL.Exceptions;

public class SyncException : Exception
{
    public SyncException()
        : base($"Sync Error")
    {
    }
}