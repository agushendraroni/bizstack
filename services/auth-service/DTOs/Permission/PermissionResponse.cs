using AuthService.Models;
using AuthService.Models.Base;

namespace AuthService.DTOs.Permission
{
    public class PermissionResponse:BaseEntity {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public PermissionType Type { get; set; }

    }
}