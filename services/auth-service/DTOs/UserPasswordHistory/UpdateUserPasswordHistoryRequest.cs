// DTOs/UserPasswordHistory/UpdateUserPasswordHistoryRequest.cs
namespace AuthService.DTOs.UserPasswordHistory
{
    public class UpdateUserPasswordHistoryRequest
    {
        public string PasswordHash { get; set; } = string.Empty;
    }
}