namespace UserService.DTOs.UserActivityLog;

public class UpdateUserActivityLogRequest
{
    public string? Activity { get; set; }
    public string? Metadata { get; set; }
}