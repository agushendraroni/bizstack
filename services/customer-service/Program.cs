using Microsoft.EntityFrameworkCore;
using CustomerService.Data;
using CustomerService.MappingProfiles;
using CustomerService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Customer Service API", Version = "v1" });
});

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseNpgsql(connectionString));

// AutoMapper
builder.Services.AddAutoMapper(typeof(CustomerMappingProfile));

// Services
builder.Services.AddScoped<ICustomerService, CustomerService.Services.CustomerService>();

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
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Service API v1");
        options.RoutePrefix = string.Empty;
    });
}

// Health endpoint
app.MapGet("/health", () => "Customer Service is running");

app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Smart migration - only migrate if no migrations applied yet
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
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
