namespace UserService.DTOs.UserActivityLog;

public class UserActivityLogResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Activity { get; set; } = default!;
    public string? Metadata { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
}