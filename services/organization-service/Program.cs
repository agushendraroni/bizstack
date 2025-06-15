using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrganizationService.Data;
using OrganizationService.Services.Implementations;
using OrganizationService.Services.Interfaces;
using OrganizationService.Validation.Company;
using SharedLibrary.Middlewares;
using SharedLibrary.Security.JWT;
using SharedLibrary.Security.Password;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- Controllers & Swagger ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- AutoMapper (opsional) ---
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// --- HttpContextAccessor ---
builder.Services.AddHttpContextAccessor();

// --- Inject OrganizationDbContext with Audit Info ---
builder.Services.AddScoped<OrganizationDbContext>(provider =>
{
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var user = httpContextAccessor.HttpContext?.User;

    var username = user?.Identity?.IsAuthenticated == true && !string.IsNullOrEmpty(user.Identity.Name)
        ? user.Identity.Name
        : "system"; // fallback if unauthenticated

    var options = provider.GetRequiredService<DbContextOptions<OrganizationDbContext>>();
    return new OrganizationDbContext(options, username); // Make sure your DbContext supports this
});

// --- DB Context ---
builder.Services.AddDbContext<OrganizationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Internal Services ---
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();

// --- JWT Authentication ---
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings == null)
    throw new InvalidOperationException("JwtSettings section is missing or invalid in configuration.");

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

// --- FluentValidation ---
builder.Services.AddFluentValidationAutoValidation()
        .AddFluentValidationClientsideAdapters()
        .AddValidatorsFromAssemblyContaining<CreateCompanyRequestValidator>();

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
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Organization Service API v1");
    options.RoutePrefix = string.Empty;
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandling();
app.UseRequestLogging();
app.UseRequestTiming();

app.MapControllers();

await app.RunAsync();
