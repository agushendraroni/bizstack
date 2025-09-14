using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.Data;
using AuthService.DTOs;
using AuthService.Models;

public class CompanyInfo
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? TenantId { get; set; }
}

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
}

namespace AuthService.Services
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly AuthDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthServiceImpl(AuthDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            // First, lookup company by CompanyCode
            var companyResponse = await GetCompanyByCodeAsync(request.CompanyCode);
            if (companyResponse == null)
                return null;

            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.TenantId == companyResponse.TenantId);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                return null;

            var accessToken = GenerateAccessToken(user, companyResponse);
            var refreshToken = GenerateRefreshToken();

            // Save refresh token
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                UserId = user.Id,
                Username = user.Username,
                CompanyCode = companyResponse.Code,
                CompanyName = companyResponse.Name,
                TenantId = companyResponse.TenantId,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            };
        }

        public async Task<LoginResponse?> RefreshTokenAsync(string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .Include(rt => rt.User)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

            if (tokenEntity == null || tokenEntity.ExpiryDate < DateTime.UtcNow)
                return null;

            // Get company info from TenantId
            CompanyInfo? company = null;
            if (tokenEntity.User.TenantId > 0)
            {
                // Try to get company info, but don't fail if service is unavailable
                try
                {
                    using var httpClient = new HttpClient();
                    var orgServiceUrl = _configuration["Services:OrganizationService"] ?? "http://localhost:5003";
                    var response = await httpClient.GetAsync($"{orgServiceUrl}/api/companies/{tokenEntity.User.TenantId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var apiResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<CompanyInfo>>(content, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        company = apiResponse?.Data;
                    }
                }
                catch
                {
                    // Continue without company info if service is unavailable
                }
            }

            var accessToken = GenerateAccessToken(tokenEntity.User, company);
            
            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                UserId = tokenEntity.User.Id,
                Username = tokenEntity.User.Username,
                CompanyCode = company?.Code,
                CompanyName = company?.Name,
                TenantId = company?.TenantId ?? tokenEntity.User.TenantId,
                Roles = tokenEntity.User.UserRoles.Select(ur => ur.Role.Name).ToList()
            };
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (tokenEntity != null)
            {
                tokenEntity.IsRevoked = true;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> RegisterAsync(string username, string password, int tenantId = 0)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
                return false;

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                TenantId = tenantId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<CompanyInfo?> GetCompanyByCodeAsync(string companyCode)
        {
            // Call Organization Service to get company by code
            using var httpClient = new HttpClient();
            var orgServiceUrl = _configuration["Services:OrganizationService"] ?? "http://localhost:5003";
            
            try
            {
                var response = await httpClient.GetAsync($"{orgServiceUrl}/api/companies/by-code/{companyCode}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<CompanyInfo>>(content, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return apiResponse?.Data;
                }
            }
            catch
            {
                // Log error
            }
            
            return null;
        }

        private string GenerateAccessToken(User user, CompanyInfo? company = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username)
            };

            if (company != null)
            {
                claims.Add(new("CompanyCode", company.Code));
                if (company.TenantId.HasValue)
                    claims.Add(new("TenantId", company.TenantId.Value.ToString()));
            }
            else if (user.TenantId > 0)
            {
                claims.Add(new("TenantId", user.TenantId.ToString()));
            }
          
            foreach (var role in user.UserRoles)
                claims.Add(new(ClaimTypes.Role, role.Role.Name));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
