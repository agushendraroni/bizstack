using SharedLibrary.Middlewares;
using SharedLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OrganizationService.Data;
using OrganizationService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = Asp.Versioning.ApiVersionReader.Combine(
        new Asp.Versioning.QueryStringApiVersionReader("version"),
        new Asp.Versioning.HeaderApiVersionReader("X-Version"),
        new Asp.Versioning.UrlSegmentApiVersionReader()
    );
}).AddMvc();

builder.Services.AddEndpointsApiExplorer();

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<OrganizationDbContext>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Organization Service API",
        Version = "v1.0"
    });
    c.AddServer(new Microsoft.OpenApi.Models.OpenApiServer
    {
        Url = "http://organization-service:5003",
        Description = "Organization Service"
    });
});

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OrganizationDbContext>(options =>
    options.UseNpgsql(connectionString));

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// JWT Authentication (using same config as auth-service)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "BizStack-AuthService",
            ValidAudience = "BizStack-Services",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a2b7fca7d51c41f087f5c2a8b64e1d4c82d4b61c58af73b9a1731a6dbfae76b9"))
        };
    });

// Services
builder.Services.AddScoped<ICompanyService, CompanyService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Organization Service API v1.0");
        options.RoutePrefix = string.Empty;
    });
}

// Health Checks
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready");

app.UseCors();
app.UseSecurityHeaders();
app.UseRouting();
app.UseAuthentication();
app.UseTenantMiddleware();
app.UseAuthorization();
app.MapControllers();

// Smart migration - only migrate if no migrations applied yet
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrganizationDbContext>();
    try 
    {
        // Check if any migrations have been applied
        var appliedMigrations = context.Database.GetAppliedMigrations();
        if (!appliedMigrations.Any())
        {
            // No migrations applied, safe to migrate
            context.Database.Migrate();
        }
        else
        {
            // Migrations exist, just ensure database can connect
            context.Database.CanConnect();
        }
    }
    catch
    {
        // If migration check fails, try to ensure database exists
        context.Database.EnsureCreated();
    }
}

app.Run();
