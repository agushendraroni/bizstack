
namespace AuthService.Services.Implementations;

using System;
using System.Linq;
using System.Threading.Tasks;
using AuthService.DTOs.Auth;
using global::AuthService.Data;
using global::AuthService.Helpers;
using global::AuthService.Models;
using Microsoft.EntityFrameworkCore;

public class AuthorizationService : AuthService.Services.Interfaces.IAuthorizationService
{
    private readonly AuthDbContext _context;
    // Removed field for static JwtService

    public AuthorizationService(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        var accessToken = JwtService.GenerateJwtToken(user);
        var refreshToken = JwtService.GenerateRefreshToken();
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        await _context.SaveChangesAsync();

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15)
        };
    }

    public async Task<LoginResponse> RefreshAsync(RefreshTokenRequest request)
    {
        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == request.RefreshToken));

        if (user == null) throw new UnauthorizedAccessException("Invalid refresh token");

        var rt = user.RefreshTokens.First(rt => rt.Token == request.RefreshToken);
        if (rt.ExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token expired");

        // Optionally: revoke old refresh token
        user.RefreshTokens.Remove(rt);

        var newRefreshToken = JwtService.GenerateRefreshToken();
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        await _context.SaveChangesAsync();

        return new LoginResponse
        {
            AccessToken = JwtService.GenerateJwtToken(user),
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15)
        };
    }

    public async Task LogoutAsync(int userId)
    {
        var user = await _context.Users.Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user != null)
        {
            user.RefreshTokens.Clear();
            await _context.SaveChangesAsync();
        }
        // Optionally, handle the case where user is not found (e.g., throw exception or log)
    }
}
