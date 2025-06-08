using AuthService.Models;

namespace AuthService.DTOs.Permission
{
    public class CreatePermissionRequest
    {
        public int CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public PermissionType Type { get; set; }
    }
}
