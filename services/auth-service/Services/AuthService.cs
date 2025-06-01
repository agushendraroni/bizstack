using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AuthService.Models;
using AuthService.Data;
using AuthService.Helpers;

namespace AuthService.Services;

public class AuthService : IAuthService
{
    private readonly AuthDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AuthDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string?> AuthenticateAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return JwtHelper.GenerateToken(user, _configuration);
    }
}
