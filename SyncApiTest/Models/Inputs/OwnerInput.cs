namespace TestGraphQL.Models.Inputs;

public record AddOwnerInput(string Name, string? Address, int? Age);

public record UpdateOwnerInput(string Name, string? Address, int? Age);