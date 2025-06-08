namespace SharedLibrary.Entities;
public abstract class BaseAuditEntity : BaseEntity
{
    public string? CreatedIpAddress { get; set; }
    public string? ChangedIpAddress { get; set; }
}