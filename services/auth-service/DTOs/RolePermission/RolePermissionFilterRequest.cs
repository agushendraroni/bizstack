using SharedLibrary.DTOs;
namespace AuthService.DTOs.RolePermission;
public class RolePermissionFilterRequest : PaginationFilter
    {
        public int? RoleId { get; set; }
        public int? PermissionId { get; set; }
    }