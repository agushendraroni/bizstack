using System;
using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace AuthService.Models
{
        public class UserPasswordHistory : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public required User User { get; set; }

        [Required]
        public required string PasswordHash { get; set; }
    }
}
