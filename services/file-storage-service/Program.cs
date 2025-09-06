using Microsoft.EntityFrameworkCore;
using FileStorageService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "File Storage Service API", Version = "v1" });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FileStorageDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "File Storage Service API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.MapGet("/health", () => "File Storage Service is running");
app.UseStaticFiles(); // Serve uploaded files
app.UseCors("AllowAll");
app.UseRouting();
app.MapControllers();

// Smart migration - only migrate if no migrations applied yet
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FileStorageDbContext>();
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
