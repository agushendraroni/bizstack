using SharedLibrary.Middlewares;
using SharedLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.MappingProfiles;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

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
    .AddDbContextCheck<UserDbContext>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "User Service API",
        Version = "v1.0"
    });
    c.AddServer(new Microsoft.OpenApi.Models.OpenApiServer
    {
        Url = "http://user-service:5002",
        Description = "User Service"
    });
});

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(connectionString));

// AutoMapper
builder.Services.AddAutoMapper(typeof(UserMappingProfile));

// Services
builder.Services.AddScoped<IUserService, UserService.Services.UserService>();

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
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "User Service API v1.0");
        options.RoutePrefix = string.Empty;
    });
}

// Health endpoint
// Health Checks
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready");

app.UseCors("AllowAll");
app.UseSecurityHeaders();
app.UseSecurityHeaders();
app.UseRouting();
app.UseTenantMiddleware();
app.UseAuthorization();
app.MapControllers();

// Smart migration - only migrate if no migrations applied yet
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
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
    catch (Exception ex)
    {
        // If migration check fails, try to ensure database exists
        context.Database.EnsureCreated();
    }
}

app.Run();
