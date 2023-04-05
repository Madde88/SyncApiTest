

namespace TestGraphQL.Models.Inputs;

public class SyncInput
{
    public List<Owner>? Owners { get; set; }  
    [Dependency(nameof(Owners))]
    public List<Dog>? Dogs { get; set; }
}