using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;


namespace OrganizationService.Models;

public class BusinessGroup : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<Company> Companies { get; set; } = new List<Company>();
}
