namespace OrganizationService.Models;
using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

public class LegalDocument : BaseEntity
{
    public Company Company { get; set; } = null!;
    public string DocumentType { get; set; } = null!;
    public string DocumentNumber { get; set; } = null!;
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Issuer { get; set; }
    public string? Description { get; set; }
    public string? FileUrl { get; set; }
}
