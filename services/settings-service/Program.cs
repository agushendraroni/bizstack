using Microsoft.EntityFrameworkCore;
using SettingsService.Data;
using SharedLibrary.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Settings Service API v1", Version = "v1" });
});

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SettingsDbContext>(options =>
    options.UseNpgsql(connectionString));

// CORS
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

// Smart migration
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SettingsDbContext>();
    try 
    {
        var appliedMigrations = context.Database.GetAppliedMigrations();
        if (!appliedMigrations.Any())
        {
            context.Database.Migrate();
        }
        else
        {
            context.Database.CanConnect();
        }
    }
    catch (Exception)
    {
        context.Database.EnsureCreated();
    }
}

// Middleware
app.UseCors("AllowAll");
app.UseExceptionHandling();
app.UseRequestLogging();
app.UseRequestTiming();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Settings Service API v1");
    options.RoutePrefix = string.Empty;
});

// Health endpoint
app.MapGet("/health", () => "Settings Service is running");

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();