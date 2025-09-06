using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace TransactionService.Models;

public class OrderItem : BaseEntity
{
    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountAmount { get; set; } = 0;

    public decimal TotalPrice { get; set; }

    [MaxLength(100)]
    public string ProductName { get; set; } = string.Empty; // Snapshot for history

    [MaxLength(50)]
    public string? ProductCode { get; set; }

    [MaxLength(50)]
    public string? Unit { get; set; }

    // Foreign Keys
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public Guid ProductId { get; set; } // Reference to Product Service
}
