namespace TestGraphQL.Models;

public class Dog : BaseEntity
    {
        [Required]
        public string? Name { get; set; }
        public int? Age { get; set; }
        public Breed? Breed { get; set; }
        public string? Color { get; set; }
        [Required]
        public Guid? OwnerId { get; set; }
        public Owner? Owner { get; set; }
    }

    public enum Breed
    {
        LabradorRetriever,
        GoldenRetriever,
        GermanShepherd,
        Bulldog,
        Poodle,
        BorderCollie
    }

    public class Owner : BaseEntity
    {
        [Required]
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? Age { get; set; }
        public List<Dog>? Dogs { get; set; }
    }
