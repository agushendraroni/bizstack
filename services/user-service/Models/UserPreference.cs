using System;
using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace UserService.Models
{
    public class UserPreference : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [MaxLength(10)]
        public string Language { get; set; } = "en";

        [MaxLength(10)]
        public string Theme { get; set; } = "light"; // "dark", "system"

        [MaxLength(50)]
        public string? Timezone { get; set; }

        public bool ReceiveNotifications { get; set; } = true;
    }
}
