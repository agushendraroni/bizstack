using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace CustomerService.Models;

public class Customer : BaseEntity
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(200)]
    public string? Address { get; set; }

    [MaxLength(50)]
    public string? City { get; set; }

    [MaxLength(10)]
    public string? PostalCode { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [MaxLength(10)]
    public string? Gender { get; set; } // Male, Female

    [MaxLength(50)]
    public string CustomerType { get; set; } = "Regular"; // Regular, VIP, Wholesale

    public decimal TotalSpent { get; set; } = 0;

    public int TotalOrders { get; set; } = 0;

    public DateTime? LastOrderDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    // Foreign Keys
    public Guid? CustomerGroupId { get; set; }
    public CustomerGroup? CustomerGroup { get; set; }

    // Navigation Properties
    public ICollection<CustomerContact> Contacts { get; set; } = new List<CustomerContact>();
}
