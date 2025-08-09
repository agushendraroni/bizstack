namespace UserService.DTOs.UserProfile;

public class CreateUserProfileRequest
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Address { get; set; } = default!;
}