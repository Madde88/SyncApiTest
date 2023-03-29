namespace TestGraphQL.Data;
using Configurations;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ConfigureRelationships();
        
        // Generate three GUIDS and place them in an arrays
        var dogids = new Guid[] {Guid.NewGuid(), Guid.NewGuid(),Guid.NewGuid(), Guid.NewGuid()};
        var ownerids = new Guid[] {Guid.NewGuid(), Guid.NewGuid()};

        // Apply configuration for the three contexts in our application
        // This will create the demo data for our GraphQL endpoint.
        builder.ApplyConfiguration(new DogConfiguration(dogids, ownerids));
        builder.ApplyConfiguration(new OwnerConfiguration(ownerids));
    }

    // Add the DbSets for each of our models we would like at our database
    public DbSet<Dog> Dogs { get; set; } = default!;
    public DbSet<Owner> Owners { get; set; } = default!;
}