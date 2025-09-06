using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserService.Data;
using UserService.Services;
using UserService.MappingProfiles;
using SharedLibrary.Middlewares;
using SharedLibrary.Security.JWT;
using SharedLibrary.Security.Password;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- Controllers & Swagger ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Service API v1", Version = "v1" });
});

// --- AutoMapper ---
builder.Services.AddAutoMapper(typeof(UserMappingProfile));

// --- HttpContextAccessor ---
builder.Services.AddHttpContextAccessor();

// --- DB Context ---
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseInMemoryDatabase("UserDb"));

// --- HTTP Clients ---
builder.Services.AddHttpClient<IOrganizationHttpClient, OrganizationHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Services:OrganizationService") ?? "http://localhost:5296");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// --- Internal Services ---
builder.Services.AddScoped<IUserService, UserService.Services.UserService>();

// --- Password Hasher ---
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();

// --- JWT Authentication (Optional) ---
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings != null)
{
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
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
}

// --- FluentValidation ---
builder.Services.AddFluentValidationAutoValidation()
        .AddFluentValidationClientsideAdapters()
        .AddValidatorsFromAssemblyContaining<Program>();

// --- CORS Policy ---
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

// --- Middleware ---
app.UseCors("AllowAll");

// --- Custom Middleware ---
app.UseExceptionHandling();
app.UseRequestLogging();
app.UseRequestTiming();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "User Service API v1");
    options.RoutePrefix = string.Empty;
});

// Health endpoint
app.MapGet("/health", () => "User Service is running");

app.UseRouting();

if (jwtSettings != null)
{
    app.UseAuthentication();
}

app.UseAuthorization();

app.MapControllers();

// --- Database Migration ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    context.Database.EnsureCreated();
}

await app.RunAsync();
