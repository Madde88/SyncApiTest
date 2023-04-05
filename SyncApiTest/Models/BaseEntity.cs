namespace TestGraphQL.Models;

public abstract class BaseEntity
{
    public BaseEntity()
    {
        DateCreated = DateTime.UtcNow;
        ServerDateUpdated = DateTime.UtcNow;
        Deleted = false;
    }
    //public abstract IEnumerable<BaseEntity> GetDependencies();
        
    [Key]
    public Guid Id { get; set; }

    public DateTime? DateCreated { get; set; }
    public DateTime? LocalDateUpdate { get; set; }
    public DateTime? ServerDateUpdated { get; set; }
    public bool? Deleted { get; set; }
    public DateTime? LastSyncedAt { get; set; }
}
