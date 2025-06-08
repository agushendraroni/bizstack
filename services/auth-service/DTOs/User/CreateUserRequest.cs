namespace AuthService.DTOs.User
{
    public class CreateUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}