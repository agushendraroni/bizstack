using AuthService.Data;
using AuthService.Helpers;
using AuthService.Services.Interfaces;
using AuthService.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthService.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// --- Services ---
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
// --- AuthService ---
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// --- JWT Authentication ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "your-secret-key"))
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("AllowAll");

// --- Middleware ---
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service API v1");
    options.RoutePrefix = string.Empty;
});

app.UseRouting();

app.UseAuthentication(); // JWT
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
