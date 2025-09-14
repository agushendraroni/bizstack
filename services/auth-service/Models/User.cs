using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace AuthService.Models
{
    public class User : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Username { get; set; } = default!;

        [Required, MaxLength(128)]
        public string PasswordHash { get; set; } = default!;

        public int LoginFailCount { get; set; } = 0;
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastFailedLoginAt { get; set; }
        public bool IsLocked { get; set; } = false;

        // Reference to User Service
        public Guid? UserProfileId { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}