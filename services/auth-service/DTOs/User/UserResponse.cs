namespace AuthService.DTOs.User
{
    /// <summary>
    /// Response DTO untuk user.
    /// </summary>
    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = default!;
        public int LoginFailCount { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastFailedLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ChangedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? ChangedBy { get; set; }
    }
}
