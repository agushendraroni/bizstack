using System;
using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace UserService.Models
{
    public class UserProfile : BaseEntity
    {
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string FullName { get; set; }

        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

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
        public string? Gender { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? City { get; set; }

        [MaxLength(50)]
        public string? Country { get; set; }

        [MaxLength(10)]
        public string? PostalCode { get; set; }
    }
}
