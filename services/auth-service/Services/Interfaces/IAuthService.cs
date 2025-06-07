using AuthService.DTOs.Auth;

namespace AuthService.Services.Interfaces;

public interface IAuthorizationService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> RefreshAsync(RefreshTokenRequest request);
    Task LogoutAsync(int userId);
}
