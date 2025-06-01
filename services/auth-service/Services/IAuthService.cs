namespace AuthService.Services;

public interface IAuthService
{
    Task<string?> AuthenticateAsync(string username, string password);
}
