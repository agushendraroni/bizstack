using AuthService.Data;
using AuthService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IAuthService, AuthService.Services.AuthService>();

builder.Services.AddAuthentication().AddJwtBearer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service API v1");
    options.RoutePrefix = string.Empty; // Agar Swagger UI tampil di root (localhost:5282/)
});

app.UseRouting();
app.UseAuthentication(); // <-- Tambahkan ini sebelum UseAuthorization
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
