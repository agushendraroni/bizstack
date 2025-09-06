using Microsoft.EntityFrameworkCore;
using TransactionService.Data;
using TransactionService.MappingProfiles;
using TransactionService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Transaction Service API", Version = "v1" });
});

// Database
builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseInMemoryDatabase("TransactionDb"));

// AutoMapper
builder.Services.AddAutoMapper(typeof(TransactionMappingProfile));

// Services
builder.Services.AddScoped<IOrderService, OrderService>();

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
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Transaction Service API v1");
        options.RoutePrefix = string.Empty;
    });
}

// Health endpoint
app.MapGet("/health", () => "Transaction Service is running");

app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
