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
builder.Services.AddDbContext<OrganizationDbContext>(options =>
    options.UseInMemoryDatabase("OrganizationDb"));

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

// Custom middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthorization();
app.MapControllers();

// Database migration
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrganizationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
