using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace CustomerService.Models;

public class CustomerContact : BaseEntity
{
    [Required, MaxLength(50)]
    public string ContactType { get; set; } = string.Empty; // Phone, Email, WhatsApp, etc.

    [Required, MaxLength(200)]
    public string ContactValue { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Label { get; set; } // Home, Work, Personal, etc.

    public bool IsPrimary { get; set; } = false;

    [MaxLength(500)]
    public string? Notes { get; set; }

    // Foreign Keys
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}
