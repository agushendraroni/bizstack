using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;


namespace OrganizationService.Models;

public class Branch : BaseEntity
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public ICollection<Division> Divisions { get; set; } = new List<Division>();

}
