using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace AuthService.Models
{
    public class RefreshToken : BaseEntity
    {
        [Required]
        public string Token { get; set; } = default!;
        
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; } = false;
        
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
