namespace TestGraphQL.Exceptions;

public class SyncException : Exception
{
    public SyncException(string message)
        : base($"Sync Error: {message}")
    {
    }
}