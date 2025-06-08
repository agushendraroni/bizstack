namespace AuthService.DTOs.UserPasswordHistory
{
    public class UserPasswordHistoryResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ChangedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? ChangedBy { get; set; }
    }
}