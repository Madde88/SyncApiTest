namespace TestGraphQL.Data;
using Configurations;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public void DisableForeignKeys()
    {
        this.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable @command1=\"ALTER TABLE ? NOCHECK CONSTRAINT ALL\"");
    }

    public void EnableForeignKeys()
    {
        this.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable @command1=\"ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL\"");
    }
    
    public void DisableForeignKeysSqlLite()
    {
        this.Database.ExecuteSqlRaw("PRAGMA foreign_keys = OFF;");
    }

    public void EnableForeignKeysSqlLite()
    {
        this.Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON;");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);

        foreach (var entityEntry in entities)
        {
            var baseEntity = entityEntry.Entity as BaseEntity;
            if (baseEntity != null && !baseEntity.DateCreated.HasValue)
            {
                baseEntity.DateCreated = DateTime.UtcNow;
                baseEntity.ServerDateUpdated = DateTime.UtcNow;
                baseEntity.LastSyncedAt = DateTime.UtcNow;
                baseEntity.Deleted = false;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
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