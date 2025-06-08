
using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace AuthService.Models
{
    public class Company : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Menu> Menus { get; set; } = new List<Menu>();
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}