using Microsoft.EntityFrameworkCore;
using OrganizationService.Data;
using OrganizationService.Services;
using OrganizationService.MappingProfiles;
using SharedLibrary.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Organization Service API", 
        Version = "v1" 
    });
});

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OrganizationDbContext>(options =>
    options.UseNpgsql(connectionString));

// AutoMapper
builder.Services.AddAutoMapper(typeof(OrganizationMappingProfile));

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

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Organization Service API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowAll");

// Health endpoint
app.MapGet("/health", () => "Organization Service is running");

// Custom middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

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
    catch (Exception ex)
    {
        // If migration check fails, try to ensure database exists
        context.Database.EnsureCreated();
    }
}

app.Run();
