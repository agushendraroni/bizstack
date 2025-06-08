namespace AuthService.DTOs.User
{
    public class UpdateUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public string? Password { get; set; } = null;
    }
}
