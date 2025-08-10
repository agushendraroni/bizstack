using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace OrganizationService.Models;

public class Division
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public Guid BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public ICollection<CostCenter> CostCenters { get; set; } = new List<CostCenter>();

    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
    public string? ChangedBy { get; set; }
}
