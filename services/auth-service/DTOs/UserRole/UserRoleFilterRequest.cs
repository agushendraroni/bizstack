using AuthService.DTOs.Common;

namespace AuthService.DTOs.UserRole
{
    public class UserRoleFilterRequest : PaginationFilter
    {
        public int? UserId { get; set; }
        public int? RoleId { get; set; }
    }
}