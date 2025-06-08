namespace SharedLibrary.Security.Middleware;
using System.Security.Claims;


public class RoleAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _requiredRole;

    public RoleAuthorizationMiddleware(RequestDelegate next, string requiredRole)
    {
        _next = next;
        _requiredRole = requiredRole;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
        if (userRole != _requiredRole)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Forbidden: Insufficient role");
            return;
        }

        await _next(context);
    }
}
