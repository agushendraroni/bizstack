
using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace OrganizationService.Models;
public class CostCenter : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    
    public Guid DivisionId { get; set; }
    public Division Division { get; set; } = null!;
}