using SharedLibrary.Entities;

namespace UserService.Models;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public Guid? OrganizationId { get; set; }
    public string? Position { get; set; }
    public string? Department { get; set; }
    
    // Navigation properties
    public UserProfile? Profile { get; set; }
    public ICollection<UserPreference> Preferences { get; set; } = new List<UserPreference>();
    public ICollection<UserActivityLog> ActivityLogs { get; set; } = new List<UserActivityLog>();
}
