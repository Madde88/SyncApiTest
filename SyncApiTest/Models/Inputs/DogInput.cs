namespace TestGraphQL.Models.Inputs;

public record AddDogInput(string Name, int? Age, Breed? Breed, string? Color, Guid OwnerId);

public record UpdateDogInput(string Name, int? Age, Breed? Breed, string? Color);