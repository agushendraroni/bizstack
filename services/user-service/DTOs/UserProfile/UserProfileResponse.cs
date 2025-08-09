namespace UserService.DTOs.UserProfile;

public class UserProfileResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Address { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
    public string? ChangedBy { get; set; }
}
