using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrganizationService.Data;
using OrganizationService.Services.Implementations;
using OrganizationService.Services.Interfaces;
using OrganizationService.Validation.Company;
using OrganizationService.Validation.Branch;
using OrganizationService.Validation.CostCenter;
using OrganizationService.Validation.LegalDocument;
using OrganizationService.Validation.Division;
using OrganizationService.Validation.JobTitle;
using OrganizationService.Validation.BusinessGroup;
using OrganizationService.DTOs.Branch;
using OrganizationService.DTOs.CostCenter;
using OrganizationService.DTOs.LegalDocument;
using OrganizationService.DTOs.Division;
using OrganizationService.DTOs.JobTitle;
using OrganizationService.MappingProfiles;
using OrganizationService.DTOs.BusinessGroup;
using SharedLibrary.Middlewares;
using SharedLibrary.Security.JWT;
using SharedLibrary.Security.Password;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- Controllers & Swagger ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- AutoMapper ---
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// --- HttpContextAccessor ---
builder.Services.AddHttpContextAccessor();

// --- Inject OrganizationDbContext with Audit Info ---
builder.Services.AddScoped<OrganizationDbContext>(provider =>
{
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var user = httpContextAccessor.HttpContext?.User;
    var username = user?.Identity?.IsAuthenticated == true && !string.IsNullOrEmpty(user.Identity.Name)
        ? user.Identity.Name
        : "system";
    var options = provider.GetRequiredService<DbContextOptions<OrganizationDbContext>>();
    return new OrganizationDbContext(options, username);
});

// --- DB Context ---
builder.Services.AddDbContext<OrganizationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Internal Services ---
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICostCenterService, CostCenterService>();
builder.Services.AddScoped<ILegalDocumentService, LegalDocumentService>();
builder.Services.AddScoped<IDivisionService, DivisionService>();
builder.Services.AddScoped<IJobTitleService, JobTitleService>();
builder.Services.AddScoped<IBusinessGroupService, BusinessGroupService>();
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();

// --- JWT Authentication ---
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings == null)
    throw new InvalidOperationException("JwtSettings section is missing or invalid in configuration.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ClockSkew = TimeSpan.Zero
        };
    });

// --- FluentValidation ---
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<CreateCompanyRequestValidator>();

// --- Validators ---
builder.Services.AddScoped<IValidator<CreateBranchRequest>, CreateBranchRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateBranchRequest>, UpdateBranchRequestValidator>();
builder.Services.AddScoped<IValidator<CreateCostCenterRequest>, CreateCostCenterRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateCostCenterRequest>, UpdateCostCenterRequestValidator>();
builder.Services.AddScoped<IValidator<CreateLegalDocumentRequest>, CreateLegalDocumentRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateLegalDocumentRequest>, UpdateLegalDocumentRequestValidator>();
builder.Services.AddScoped<IValidator<CreateDivisionRequest>, CreateDivisionRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateDivisionRequest>, UpdateDivisionRequestValidator>();
builder.Services.AddScoped<IValidator<CreateJobTitleRequest>, CreateJobTitleRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateJobTitleRequest>, UpdateJobTitleRequestValidator>();
builder.Services.AddScoped<IValidator<CreateBusinessGroupRequest>, CreateBusinessGroupRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateBusinessGroupRequest>, UpdateBusinessGroupRequestValidator>();

// --- Mapping Profiles ---
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<BranchProfile>();
    cfg.AddProfile<CostCenterProfile>();
    cfg.AddProfile<LegalDocumentProfile>();
    cfg.AddProfile<DivisionProfile>();
    cfg.AddProfile<JobTitleProfile>();
    cfg.AddProfile<BusinessGroupProfile>();
});

// --- CORS Policy ---
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

// --- Middleware ---
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Organization Service API v1");
    options.RoutePrefix = string.Empty;
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// --- Custom Middleware ---
app.UseExceptionHandling();
app.UseRequestLogging();
app.UseRequestTiming();

await app.RunAsync();
