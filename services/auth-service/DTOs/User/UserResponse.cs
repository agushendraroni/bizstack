namespace AuthService.DTOs.User
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ChangedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? ChangedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int LoginFailCount { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastFailedLoginAt { get; set; }
    }
}