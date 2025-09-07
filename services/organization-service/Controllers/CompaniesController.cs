using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using OrganizationService.DTOs;
using OrganizationService.Services;

namespace OrganizationService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCompanyById(Guid id)
    {
        var result = await _companyService.GetCompanyByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("code/{code}")]
    public async Task<IActionResult> GetCompanyByCode(string code)
    {
        var result = await _companyService.GetCompanyByCodeAsync(code);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto createCompanyDto)
    {
        var result = await _companyService.CreateCompanyAsync(createCompanyDto);
        return result.IsSuccess ? CreatedAtAction(nameof(GetCompanyById), new { id = result.Data?.Id }, result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] UpdateCompanyDto updateCompanyDto)
    {
        var result = await _companyService.UpdateCompanyAsync(id, updateCompanyDto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        var result = await _companyService.DeleteCompanyAsync(id);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    private int? GetTenantId()
    {
        return null;
    }

    private Guid? GetUserId()
    {
        return null;
    }
}
