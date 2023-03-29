namespace TestGraphQL.Repository;

public class OwnerRepository : IOwnerRepository
{
    private readonly ApplicationDbContext _dbContext;
    public OwnerRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
    }
    public Task<List<Owner>> GetOwnersAsync() =>
        _dbContext.Owners.Include(o => o.Dogs).Where(a => a.Deleted == false).ToListAsync();

    public async Task<Owner> GetOwnerByIdAsync(Guid id)
    {
       var owner = await _dbContext.Owners.Include(o => o.Dogs).Where(a => a.Id == id).FirstOrDefaultAsync();

       if (owner == null) throw new EntityDontExistsException(nameof(owner));

       return owner;
    }
     
    
    public async Task<OwnerPayload> AddOwnerAsync(AddOwnerInput input)
    {
        var owner = new Owner
        {
            Name = input.Name,
            Address = input.Address,
            Age = input.Age,
        }; 

        _dbContext.Owners.Add(owner);
        await _dbContext.SaveChangesAsync();

        return new OwnerPayload(owner);
    }
    
    public async Task<OwnerPayload> UpdateOwnerAsync(Guid id, UpdateOwnerInput input)
    {
        var owner = await _dbContext.Owners.Include(o => o.Dogs).Where(a => a.Id == id).FirstOrDefaultAsync();

        if (owner == null) throw new EntityDontExistsException(nameof(owner));
        
         owner.Name = input.Name;
        if (input.Address != null) owner.Address = input.Address;
        if (input.Age != null) owner.Age = input.Age;
        
        owner.ServerDateUpdated = DateTime.UtcNow;
            
        _dbContext.Update(owner);
        await _dbContext.SaveChangesAsync();
        return new OwnerPayload(owner);

    }

    public async Task<bool> DeleteOwnerAsync(Guid id)
    {
        var owner = await _dbContext.Owners.Where(a => a.Id == id).FirstOrDefaultAsync();
        if (owner == null) throw new EntityDontExistsException(nameof(owner));
        
        owner.ServerDateUpdated = DateTime.UtcNow;
        owner.Deleted = true;
        
        _dbContext.Update(owner);
        await _dbContext.SaveChangesAsync();
        return true;
    }
    
    public async Task<OwnerPayload> AddDogToOwnerAsync(Guid ownerId, Guid dogId)
    {
        var owner = await _dbContext.Owners.Include(o => o.Dogs).SingleOrDefaultAsync(o => o.Id == ownerId);
        if (owner == null) throw new EntityDontExistsException(nameof(Owner));
    
        var dog = await _dbContext.Dogs.FindAsync(dogId);
        if (dog == null) throw new EntityDontExistsException(nameof(Dog));

        if (owner.Dogs?.Find(a => a.Id == dog.Id) != null) throw new EntityAlreadyExistsException(nameof(Dog));

        if (owner.Dogs == null)
            owner.Dogs = new List<Dog>();

        owner.Dogs.Add(dog);
        await _dbContext.SaveChangesAsync();
    
        return new OwnerPayload(owner);
    }
}

public interface IOwnerRepository
{
    Task<List<Owner>> GetOwnersAsync();
    Task<Owner> GetOwnerByIdAsync(Guid id);
    Task<OwnerPayload> AddOwnerAsync(AddOwnerInput input);
    Task<OwnerPayload> UpdateOwnerAsync(Guid id, UpdateOwnerInput input);
    Task<bool> DeleteOwnerAsync(Guid id);
    Task<OwnerPayload> AddDogToOwnerAsync(Guid ownerId, Guid dogId);
}
