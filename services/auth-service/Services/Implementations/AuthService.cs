namespace AuthService.Services.Implementations;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuthService.DTOs.Auth;
using AuthService.Data;
using AuthService.Models;
using SharedLibrary.Security;
using SharedLibrary.Security.JWT;
using SharedLibrary.Security.Password;
using System.Security.Claims;
using AuthService.Validation;
using System.ComponentModel.DataAnnotations;
using AuthService.Validation.Auth;
using FluentValidation;

public class AuthorizationService : AuthService.Services.Interfaces.IAuthorizationService
{
     private readonly AuthDbContext _context;
    private readonly IJwtTokenService _jwtService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly IValidator<RefreshTokenRequest> _refreshTokenValidator;

    public AuthorizationService(
        AuthDbContext context,
        IJwtTokenService jwtService,
        IPasswordHasher passwordHasher,
        IValidator<LoginRequest> loginValidator,
        IValidator<RefreshTokenRequest> refreshTokenValidator)
    {
        _context = context;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
        _loginValidator = loginValidator;
        _refreshTokenValidator = refreshTokenValidator;
    }

    // LoginAsync
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

    
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("CompanyId", user.CompanyId.ToString())
        };
        claims.AddRange(user.UserRoles.Select(ur => new Claim("RoleId", ur.RoleId.ToString())));

        var accessToken = _jwtService.GenerateAccessToken(claims);
        var refreshToken = _jwtService.GenerateRefreshToken();
        if (user.RefreshTokens == null)
        {
            user.RefreshTokens = new List<RefreshToken>();
        }
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

    // RefreshAsync
    public async Task<LoginResponse> RefreshAsync(RefreshTokenRequest request)
    {
        var validationResult = await _refreshTokenValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == request.RefreshToken));

        if (user == null) throw new UnauthorizedAccessException("Invalid refresh token");

        var rt = user.RefreshTokens.First(rt => rt.Token == request.RefreshToken);
        if (rt.ExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token expired");

        user.RefreshTokens.Remove(rt);

        var newRefreshToken = _jwtService.GenerateRefreshToken();
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        await _context.SaveChangesAsync();

         var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("CompanyId", user.CompanyId.ToString())
        };
        claims.AddRange(user.UserRoles.Select(ur => new Claim("RoleId", ur.RoleId.ToString())));

        return new LoginResponse
        {
            AccessToken = _jwtService.GenerateAccessToken(claims),
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
    }
}
