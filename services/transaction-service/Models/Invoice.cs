using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace TransactionService.Models;

public class Invoice : BaseEntity
{
    [Required, MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

    public DateTime DueDate { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Draft"; // Draft, Sent, Paid, Overdue, Cancelled

    public decimal SubTotal { get; set; }

    public decimal TaxAmount { get; set; } = 0;

    public decimal DiscountAmount { get; set; } = 0;

    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; } = 0;

    public decimal RemainingAmount { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    // Foreign Keys
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
}
