namespace UserService.DTOs.UserPreference;

public class CreateUserPreferenceRequest
{
    public Guid UserId { get; set; }
    public string Language { get; set; } = default!;
    public string Theme { get; set; } = default!;
    public bool ReceiveNotifications { get; set; }
}