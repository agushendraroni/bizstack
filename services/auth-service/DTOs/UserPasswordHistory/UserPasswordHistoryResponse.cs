namespace AuthService.DTOs.UserPasswordHistory
{
    public class UserPasswordHistoryResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PasswordHash { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}