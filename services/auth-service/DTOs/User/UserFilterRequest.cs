using AuthService.DTOs.Common;

namespace AuthService.DTOs.User
{
    public class UserFilterRequest : PaginationFilter
    {
        public string? Username { get; set; }
        public int? CompanyId { get; set; }
    }
}