using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AuthService.Models.Base;

namespace AuthService.Models
{
    public class Permission: BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty; // e.g. "user:create", "menu:dashboard"

        [MaxLength(255)]
        public string? Description { get; set; }

        [Required]
        public PermissionType Type { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
        public ICollection<MenuPermission> MenuPermissions { get; set; } = new List<MenuPermission>();
    }
}
