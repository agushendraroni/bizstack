using AuthService.Models;

namespace AuthService.DTOs.RolePermission
{
      public class CreateRolePermissionRequest
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}
