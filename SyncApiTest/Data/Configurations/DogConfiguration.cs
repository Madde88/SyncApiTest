namespace TestGraphQL.Data.Configurations;

    public class DogConfiguration : IEntityTypeConfiguration<Dog>
    {
        private readonly Guid[] _ids;
        private readonly Guid[] _ownerIds;

        public DogConfiguration(Guid[] ids, Guid[] ownerIds)
        {
            _ids = ids;
            _ownerIds = ownerIds;
        }

        public void Configure(EntityTypeBuilder<Dog> builder)
        {
            builder
                .HasData(
                    new Dog()
                    {
                        Id = _ids[0],
                        Name = "Lucy",
                        Age = 9,
                        OwnerId = _ownerIds[0],
                        Breed = Breed.LabradorRetriever,
                        Color = "Black"
                    },
                    new Dog()
                    {
                        Id = _ids[1],
                        Name = "Ruby",
                        Age = 3,
                        OwnerId = _ownerIds[0],
                        Breed = Breed.GoldenRetriever,
                        Color = "Golden"
                    },
                    new Dog()
                    {
                        Id = _ids[2],
                        Name = "Max",
                        Age = 5,
                        OwnerId = _ownerIds[1],
                        Breed = Breed.GermanShepherd,
                        Color = "Brown"
                    },
                    new Dog()
                    {
                        Id = _ids[3],
                        Name = "Buddy",
                        Age = 2,
                        OwnerId = _ownerIds[1],
                        Breed = Breed.Bulldog,
                        Color = "White"
                    }
                );
        }
    }