namespace TestGraphQL.Models.Payloads;

public class OwnerPayload
{
    public OwnerPayload(Owner owner)
    {
        Owner = owner;
    }

    public Owner Owner { get; }
}