using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace CustomerService.Models;

public class CustomerGroup : BaseEntity
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public decimal? DiscountPercentage { get; set; } = 0;

    public decimal? MinimumSpent { get; set; } = 0;

    [MaxLength(50)]
    public string? Color { get; set; } // For UI display

    // Navigation Properties
    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
