using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace AuthService.Models
{
    public class Role : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = default!;
        
        public string? Description { get; set; }
        
        public Guid? CompanyId { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
