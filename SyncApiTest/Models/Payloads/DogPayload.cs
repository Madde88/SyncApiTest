namespace TestGraphQL.Models.Payloads;

public class DogPayload
{
    public DogPayload(Dog dog)
    {
        Dog = dog;
    }

    public Dog Dog { get; }
}