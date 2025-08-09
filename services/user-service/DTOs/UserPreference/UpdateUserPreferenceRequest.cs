namespace UserService.DTOs.UserPreference;
public class UpdateUserPreferenceRequest
{
    public string? Language { get; set; }
    public string? Theme { get; set; }
    public bool? ReceiveNotifications { get; set; }
}
