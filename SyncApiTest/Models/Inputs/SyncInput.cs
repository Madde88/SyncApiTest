namespace TestGraphQL.Models.Inputs;

public class SyncInput
{
    public List<Dog>? Dogs { get; set; }      
    public List<Owner>? Owners { get; set; }    
}