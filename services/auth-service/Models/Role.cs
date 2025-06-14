using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace AuthService.Models
{
    public class Role : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int CompanyId { get; set; }


        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
