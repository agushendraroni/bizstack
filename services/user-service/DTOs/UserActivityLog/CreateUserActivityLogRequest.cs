namespace UserService.DTOs.UserActivityLog;

public class CreateUserActivityLogRequest
{
    public Guid UserId { get; set; }
    public string Activity { get; set; } = default!;
    public string? Metadata { get; set; }
}
