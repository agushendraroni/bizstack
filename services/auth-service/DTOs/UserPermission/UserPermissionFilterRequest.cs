using AuthService.DTOs.Common;

namespace AuthService.DTOs.UserPermission
{
     public class UserPermissionFilterRequest : PaginationFilter
    {
        public int? UserId { get; set; }
        public int? PermissionId { get; set; }
    }
}