namespace TestGraphQL.Models.Payloads;

public class SyncPayload
{
    public SyncPayload()
    {
        Dogs = new List<Dog>();
        Owners = new List<Owner>();
    }
    
    public SyncPayload(List<Dog> dogs, List<Owner> owners)
    {
        Dogs = dogs;
        Owners = owners;
    }

    public List<Dog> Dogs { get; set; }
    public List<Owner> Owners { get; set; }
}