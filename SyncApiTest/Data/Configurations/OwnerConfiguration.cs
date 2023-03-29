namespace TestGraphQL.Data.Configurations;

public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    private Guid[] _ids;

    public OwnerConfiguration(Guid[] ids)
    {
        _ids = ids;
    }

    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.HasData(
            new Owner()
            {
                Id = _ids[0],
                Name = "John Doe",
                Age = 35
            },
            new Owner()
            {
                Id = _ids[1],
                Name = "Jane Doe",
                Age = 30
            }
        );
    }
}
