namespace OrganizationService.Models;
using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;


public class JobTitle : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public Company Company { get; set; } = null!;


}
