using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrganizationService.Data;
using OrganizationService.DTOs.Company;
using OrganizationService.Services.Implementations;
using OrganizationService.Services.Interfaces;
using OrganizationService.Validation.Company;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrganizationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation()
        .AddFluentValidationClientsideAdapters()
        .AddValidatorsFromAssemblyContaining<CreateCompanyRequestValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
