using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace TransactionService.Models;

public class Payment : BaseEntity
{
    [Required, MaxLength(50)]
    public string PaymentNumber { get; set; } = string.Empty;

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    public decimal Amount { get; set; }

    [Required, MaxLength(20)]
    public string PaymentMethod { get; set; } = string.Empty; // Cash, Transfer, Credit Card, E-Wallet

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded

    [MaxLength(100)]
    public string? ReferenceNumber { get; set; } // Bank reference, transaction ID

    [MaxLength(500)]
    public string? Notes { get; set; }

    // Foreign Keys
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
}
