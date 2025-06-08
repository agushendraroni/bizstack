namespace AuthService.DTOs.UserPasswordHistory
{
   public class CreateUserPasswordHistoryRequest
    {
        public int UserId { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
    }
}
