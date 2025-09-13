using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using OrganizationService.Data;
using OrganizationService.Models;
using SharedLibrary.DTOs;

namespace OrganizationService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LegalDocumentsController : ControllerBase
{
    private readonly OrganizationDbContext _context;

    public LegalDocumentsController(OrganizationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetLegalDocuments()
    {
        var documents = await _context.LegalDocuments
            .Include(ld => ld.Company)
            .ToListAsync();
        return Ok(ApiResponse<List<LegalDocument>>.Success(documents));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLegalDocument(Guid id)
    {
        var document = await _context.LegalDocuments
            .Include(ld => ld.Company)
            .FirstOrDefaultAsync(ld => ld.Id == id);
        
        if (document == null)
            return NotFound(ApiResponse<LegalDocument>.Error("Legal document not found"));
        
        return Ok(ApiResponse<LegalDocument>.Success(document));
    }

    [HttpGet("company/{companyId}")]
    public async Task<IActionResult> GetLegalDocumentsByCompany(int companyId)
    {
        var documents = await _context.LegalDocuments
            .Where(ld => ld.CompanyId == companyId)
            .Include(ld => ld.Company)
            .ToListAsync();
        
        return Ok(ApiResponse<List<LegalDocument>>.Success(documents));
    }

    [HttpGet("type/{documentType}")]
    public async Task<IActionResult> GetLegalDocumentsByType(string documentType)
    {
        var documents = await _context.LegalDocuments
            .Where(ld => ld.DocumentType == documentType)
            .Include(ld => ld.Company)
            .ToListAsync();
        
        return Ok(ApiResponse<List<LegalDocument>>.Success(documents));
    }

    [HttpGet("expiring")]
    public async Task<IActionResult> GetExpiringDocuments([FromQuery] int days = 30)
    {
        var expiryDate = DateTime.UtcNow.AddDays(days);
        var documents = await _context.LegalDocuments
            .Where(ld => ld.ExpiryDate.HasValue && ld.ExpiryDate <= expiryDate)
            .Include(ld => ld.Company)
            .OrderBy(ld => ld.ExpiryDate)
            .ToListAsync();
        
        return Ok(ApiResponse<List<LegalDocument>>.Success(documents));
    }

    [HttpPost]
    public async Task<IActionResult> CreateLegalDocument([FromBody] CreateLegalDocumentDto dto)
    {
        var company = await _context.Companies.FindAsync(dto.CompanyId);
        if (company == null)
            return BadRequest(ApiResponse<LegalDocument>.Error("Company not found"));

        var document = new LegalDocument
        {
            CompanyId = dto.CompanyId,
            DocumentType = dto.DocumentType,
            DocumentNumber = dto.DocumentNumber,
            IssueDate = dto.IssueDate,
            ExpiryDate = dto.ExpiryDate,
            Issuer = dto.Issuer,
            Description = dto.Description,
            FileUrl = dto.FileUrl
        };

        _context.LegalDocuments.Add(document);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetLegalDocument), new { id = document.Id }, ApiResponse<LegalDocument>.Success(document));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLegalDocument(Guid id, [FromBody] UpdateLegalDocumentDto dto)
    {
        var document = await _context.LegalDocuments.FindAsync(id);
        if (document == null)
            return NotFound(ApiResponse<LegalDocument>.Error("Legal document not found"));

        if (!string.IsNullOrEmpty(dto.DocumentType)) document.DocumentType = dto.DocumentType;
        if (!string.IsNullOrEmpty(dto.DocumentNumber)) document.DocumentNumber = dto.DocumentNumber;
        if (dto.IssueDate.HasValue) document.IssueDate = dto.IssueDate;
        if (dto.ExpiryDate.HasValue) document.ExpiryDate = dto.ExpiryDate;
        if (dto.Issuer != null) document.Issuer = dto.Issuer;
        if (dto.Description != null) document.Description = dto.Description;
        if (dto.FileUrl != null) document.FileUrl = dto.FileUrl;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<LegalDocument>.Success(document));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLegalDocument(Guid id)
    {
        var document = await _context.LegalDocuments.FindAsync(id);
        if (document == null)
            return NotFound(ApiResponse<LegalDocument>.Error("Legal document not found"));

        _context.LegalDocuments.Remove(document);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("Legal document deleted successfully"));
    }
}

public class CreateLegalDocumentDto
{
    public int CompanyId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Issuer { get; set; }
    public string? Description { get; set; }
    public string? FileUrl { get; set; }
}

public class UpdateLegalDocumentDto
{
    public string? DocumentType { get; set; }
    public string? DocumentNumber { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Issuer { get; set; }
    public string? Description { get; set; }
    public string? FileUrl { get; set; }

    private Guid? GetUserId()
    {
        return null;
    }
}