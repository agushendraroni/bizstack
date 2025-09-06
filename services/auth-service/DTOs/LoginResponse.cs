namespace AuthService.DTOs
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = default!;
        public Guid? CompanyId { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
