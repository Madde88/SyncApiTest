
namespace TestGraphQL.Data;

public class Mutation
{
    private readonly IDogRepository _dogRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly ISyncService _syncService;

    public Mutation(IDogRepository dogRepository, IOwnerRepository ownerRepository, ISyncService syncService)
    {
        _dogRepository = dogRepository;
        _ownerRepository = ownerRepository;
        _syncService = syncService;
    }

    [Error(typeof(EntityDontExistsException))]
    public async Task<DogPayload> AddDogAsync(AddDogInput input)
    {
        return await _dogRepository.AddDogAsync(input);
    }
    
    [Error(typeof(EntityDontExistsException))]
    public async Task<DogPayload> UpdateDogAsync([ID] Guid id, UpdateDogInput input)
    {
        return await _dogRepository.UpdateDogAsync(id,input);
    }
    
    [Error(typeof(EntityDontExistsException))]
    public async Task<bool> DeleteDogAsync([ID] Guid id)
    {
        return await _dogRepository.DeleteDogAsync(id);
    }
    
    [Error(typeof(EntityDontExistsException))]
    public async Task<OwnerPayload> AddOwnerAsync(AddOwnerInput input)
    {
        return await _ownerRepository.AddOwnerAsync(input);
    }

    [Error(typeof(EntityDontExistsException))]
    public async Task<OwnerPayload> UpdateOwnerAsync([ID] Guid id, UpdateOwnerInput input)
    {
        return await _ownerRepository.UpdateOwnerAsync(id,input);
    }

    [Error(typeof(EntityDontExistsException))]
    public async Task<bool> DeleteOwnerAsync([ID] Guid id)
    {
        return await _ownerRepository.DeleteOwnerAsync(id);
    }
    
    [Error(typeof(EntityAlreadyExistsException))]
    [Error(typeof(EntityDontExistsException))]
    public async Task<OwnerPayload> AddDogToOwnerAsync([ID] Guid ownerId, Guid dogId)
    {
        return await _ownerRepository.AddDogToOwnerAsync(ownerId, dogId);
    }
    
    [Error(typeof(SyncException))]
    public async Task<SyncPayload> SyncDateAsync(SyncInput input)
    {
        try
        {
            return await _syncService.HandleSync(input);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new SyncException();
        }
        
    }

}