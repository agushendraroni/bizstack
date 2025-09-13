using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace OrganizationService.Models;

public class Division : BaseEntity
{
    public string Name { get; set; } = null!;
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    public Guid BranchId { get; set; }
    public Branch Branch { get; set; } = null!;
    public ICollection<CostCenter> CostCenters { get; set; } = new List<CostCenter>();
}
