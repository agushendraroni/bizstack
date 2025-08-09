using System;
using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace UserService.Models
{
    public class UserActivityLog : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Activity { get; set; } // "Login", "UpdateProfile", etc.

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? IpAddress { get; set; }

        [MaxLength(300)]
        public string? UserAgent { get; set; }
    }
}
