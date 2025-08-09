using SharedLibrary.DTOs;
namespace UserService.DTOs.UserActivityLog;


public class UserActivityLogFilterRequest : PaginationFilter
{
    public Guid? UserId { get; set; }
    public string? Activity { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }
}
