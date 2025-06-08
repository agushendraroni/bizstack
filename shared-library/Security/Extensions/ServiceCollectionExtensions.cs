using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Security.JWT;
using SharedLibrary.Security.Password;
using SharedLibrary.Security.Services;


namespace SharedLibrary.Security.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        return services;
    }
}
