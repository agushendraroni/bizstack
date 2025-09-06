using AuthService.DTOs;

namespace AuthService.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<LoginResponse?> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(string refreshToken);
        Task<bool> RegisterAsync(string username, string password, Guid? companyId = null);
    }
}
