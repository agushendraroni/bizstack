using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SharedLibrary.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Extract TenantId and UserId from JWT token
        var (tenantId, userId) = GetClaimsFromToken(context);
        
        if (tenantId.HasValue)
        {
            context.Request.Headers["X-Tenant-Id"] = tenantId.Value.ToString();
        }
        
        if (userId.HasValue)
        {
            context.Request.Headers["X-User-Id"] = userId.Value.ToString();
        }

        await _next(context);
    }

    private (int? tenantId, Guid? userId) GetClaimsFromToken(HttpContext context)
    {
        try
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
                return (null, null);

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            
            if (!handler.CanReadToken(token))
                return (null, null);

            var jsonToken = handler.ReadJwtToken(token);
            
            // Extract TenantId
            var tenantIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "TenantId");
            int? tenantId = null;
            if (tenantIdClaim != null && int.TryParse(tenantIdClaim.Value, out var tId))
                tenantId = tId;

            // Extract UserId
            var userIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Guid? userId = null;
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var uId))
                userId = uId;

            return (tenantId, userId);
        }
        catch
        {
            return (null, null);
        }
    }
}