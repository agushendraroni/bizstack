using AuthService.Data;
using AuthService.Helpers;
using AuthService.Services.Interfaces;
using AuthService.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthService.MappingProfiles;
using SharedLibrary.Middlewares;
using SharedLibrary.Security.JWT;

using SharedLibrary.Security.Password;
using FluentValidation;
using FluentValidation.AspNetCore;
using AuthService.Validation;
using AuthService.Validation.Auth;
using AuthService.Validation.Menu;
using AuthService.Validation.Permission;
using AuthService.Validation.RolePermission;
using AuthService.Validation.Role;

var builder = WebApplication.CreateBuilder(args);

// --- Add Controllers & Swagger ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Register IHttpContextAccessor ---
builder.Services.AddHttpContextAccessor();

// --- Register AuthDbContext with audit support ---
builder.Services.AddScoped<AuthDbContext>(provider =>
{
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var user = httpContextAccessor.HttpContext?.User;

    var username = user?.Identity?.IsAuthenticated == true && !string.IsNullOrEmpty(user.Identity.Name)
        ? user.Identity.Name
        : "system"; // fallback default

    var options = provider.GetRequiredService<DbContextOptions<AuthDbContext>>();
    return new AuthDbContext(options, username);
});

// --- Register AutoMapper ---
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// --- Register Internal Services ---
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

// --- Bind & Register JwtSettings ---
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings == null)
    throw new InvalidOperationException("JwtSettings section is missing or invalid in configuration.");

// --- Register SharedLibrary JWT Service ---
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// --- JWT Authentication ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ClockSkew = TimeSpan.Zero
        };
    });

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();

var app = builder.Build();

// --- Middleware ---
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service API v1");
    options.RoutePrefix = string.Empty;
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// --- Custom Middleware ---
app.UseExceptionHandling();
app.UseRequestLogging();
app.UseRequestTiming();

await app.RunAsync();
