using System;
using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace UserService.Models
{
    public class UserProfile : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } // Same as AuthService UserId

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? AvatarUrl { get; set; }

        [MaxLength(500)]
        public string? Bio { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; } // "Male", "Female", etc.
    }
}
