namespace TestGraphQL.Models.Payloads;

public class SyncPayload
{
    public SyncPayload(List<Dog> dogs, List<Owner> owners)
    {
        Dogs = dogs;
        Owners = owners;
    }

    public List<Dog> Dogs { get; }
    public List<Owner> Owners { get; }
}