namespace TestGraphQL.Data;

public class Query
{
    private readonly IDogRepository _dogRepository;
    private readonly IOwnerRepository _ownerRepository;
    public Query(IDogRepository dogRepository, IOwnerRepository ownerRepository)
    {
        _dogRepository = dogRepository;
        _ownerRepository = ownerRepository;
    }
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public Task<List<Dog>> GetDogs() =>
        _dogRepository.GetDogsAsync();
    
    public Task<Dog> GetDogById([ID] Guid id) =>
        _dogRepository.GetDogByIdAsync(id);
    
    public Task<List<Owner>> GetOwners() =>
        _ownerRepository.GetOwnersAsync();
    
    public Task<Owner> GetOwnerById([ID] Guid id) =>
        _ownerRepository.GetOwnerByIdAsync(id);
}