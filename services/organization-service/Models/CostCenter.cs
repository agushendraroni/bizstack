
using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace OrganizationService.Models;
public class CostCenter : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    public Division Division { get; set; } = null!;

}