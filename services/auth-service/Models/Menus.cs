using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedLibrary.Entities;

namespace AuthService.Models
{
    public class Menu : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(255)]
        public string? Url { get; set; }

        [MaxLength(100)]
        public string? Icon { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Menu? Parent { get; set; }

        public ICollection<Menu> Children { get; set; } = new List<Menu>();

        public int OrderIndex { get; set; } = 0;

        public ICollection<MenuPermission> MenuPermissions { get; set; } = new List<MenuPermission>();
    }
}
