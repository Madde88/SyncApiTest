namespace TestGraphQL.Models;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        DateCreated = DateTime.UtcNow;
        Deleted = false;
    }
        
    [Key]
    public Guid Id { get; set; }

    private DateTime DateCreated { get; set; }
    public DateTime? LocalDateUpdate { get; set; }
    public DateTime? ServerDateUpdated { get; set; }
    public bool Deleted { get; set; }
    public DateTime? LastSyncedAt { get; set; }
}
