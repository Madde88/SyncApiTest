namespace TestGraphQL.Data.Configurations;

public static class Relationships
{
    public static void ConfigureRelationships(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dog>()
            .HasOne<Owner>(d => d.Owner)
            .WithMany(o => o.Dogs)
            .HasForeignKey(d => d.OwnerId);
    }
}