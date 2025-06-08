using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AuthService.Models.Base;

namespace AuthService.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; } = default!;

        [Required]
        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;

        [Required, MaxLength(128)]
        public string PasswordHash { get; set; } = default!;

        public int LoginFailCount { get; set; } = 0;
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastFailedLoginAt { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<UserPasswordHistory> PasswordHistories { get; set; } = new List<UserPasswordHistory>();
    }
}