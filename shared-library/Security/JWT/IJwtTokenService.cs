namespace SharedLibrary.Security.JWT
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public interface IJwtTokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
