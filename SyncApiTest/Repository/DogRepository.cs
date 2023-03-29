namespace TestGraphQL.Repository;
using GraphQL.EntityFramework;

public class DogRepository : IDogRepository
{
    private readonly ApplicationDbContext _dbContext;
    public DogRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
    }
    public Task<List<Dog>> GetDogsAsync() =>
        _dbContext.Dogs.Include(a => a.Owner).Where(a => a.Deleted == false).ToListAsync();

    public async Task<Dog> GetDogByIdAsync(Guid id)
    {
       var dog = await _dbContext.Dogs.Include(a => a.Owner).Where(a => a.Id == id).FirstOrDefaultAsync();

       if (dog == null) throw new EntityDontExistsException(nameof(dog));

       return dog;
    }
     
    
    public async Task<DogPayload> AddDogAsync(AddDogInput input)
    {
        var owner = await _dbContext.Owners.Where(a => a.Id == input.OwnerId).FirstOrDefaultAsync();
        if (owner == null) throw new EntityDontExistsException(nameof(owner));
        
        var dog = new Dog
        {
            Name = input.Name,
            Age = input.Age,
            OwnerId = input.OwnerId,
            Breed = input.Breed,
            Color = input.Color
        };

        _dbContext.Dogs.Add(dog);
        await _dbContext.SaveChangesAsync();
        
        return new DogPayload(dog);
    }
    
    public async Task<DogPayload> UpdateDogAsync(Guid id, UpdateDogInput input)
    {
        var dog = await _dbContext.Dogs.Include(a => a.Owner).Where(a => a.Id == id).FirstOrDefaultAsync();

        if (dog == null) throw new EntityDontExistsException(nameof(dog));
        
        dog.Name = input.Name;
        if (input.Age != null) dog.Age = input.Age;
        if (input.Breed != null) dog.Breed = input.Breed;
        if (input.Color != null) dog.Color = input.Color;
        
        dog.ServerDateUpdated = DateTime.UtcNow;
            
        _dbContext.Update(dog);
        await _dbContext.SaveChangesAsync();
        return new DogPayload(dog);

    }

    public async Task<bool> DeleteDogAsync(Guid id)
    {
        var dog = await _dbContext.Dogs.Where(a => a.Id == id).FirstOrDefaultAsync();
        if (dog == null) throw new EntityDontExistsException(nameof(dog));
        
        dog.ServerDateUpdated = DateTime.UtcNow;
        dog.Deleted = true;
        
        _dbContext.Update(dog);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}


public interface IDogRepository
{
    Task<List<Dog>> GetDogsAsync();
    Task<Dog> GetDogByIdAsync(Guid id);
    Task<DogPayload> AddDogAsync(AddDogInput input);
    Task<DogPayload> UpdateDogAsync(Guid id, UpdateDogInput input);
    Task<bool> DeleteDogAsync(Guid id);
}