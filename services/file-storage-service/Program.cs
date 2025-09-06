using Microsoft.EntityFrameworkCore;
using FileStorageService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "File Storage Service API", Version = "v1" });
});

builder.Services.AddDbContext<FileStorageDbContext>(options =>
    options.UseInMemoryDatabase("FileStorageDb"));

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
app.Run();
