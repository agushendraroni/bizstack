using SharedLibrary.DTOs;


namespace UserService.DTOs.UserProfile;
public class UserProfileFilterRequest : PaginationFilter
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }
    public bool? IsActive { get; set; }
}
