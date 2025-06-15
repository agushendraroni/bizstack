namespace OrganizationService.Models;
using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;


public class JobTitle : BaseEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;


}
