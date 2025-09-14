using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OrganizationService.DTOs;
using OrganizationService.Services;
using System.Security.Claims;

namespace OrganizationService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCompanies()
    {
        var tenantId = GetTenantId();
        var result = await _companyService.GetAllCompaniesAsync(tenantId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{tenantId}")]
    public async Task<IActionResult> GetCompanyById(int tenantId)
    {
        var result = await _companyService.GetCompanyByIdAsync(tenantId);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("code/{code}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCompanyByCode(string code)
    {
        var result = await _companyService.GetCompanyByCodeAsync(code);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto createCompanyDto)
    {
        var result = await _companyService.CreateCompanyAsync(createCompanyDto);
        return result.IsSuccess ? CreatedAtAction(nameof(GetCompanyById), new { tenantId = result.Data?.TenantId }, result) : BadRequest(result);
    }

    [HttpPut("{tenantId}")]
    public async Task<IActionResult> UpdateCompany(int tenantId, [FromBody] UpdateCompanyDto updateCompanyDto)
    {
        var result = await _companyService.UpdateCompanyAsync(tenantId, updateCompanyDto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{tenantId}")]
    public async Task<IActionResult> DeleteCompany(int tenantId)
    {
        var result = await _companyService.DeleteCompanyAsync(tenantId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    private int? GetTenantId()
    {
        var tenantIdClaim = User.FindFirst("TenantId")?.Value;
        return int.TryParse(tenantIdClaim, out var tenantId) ? tenantId : null;
    }

    private Guid? GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
