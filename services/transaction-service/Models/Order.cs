using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace TransactionService.Models;

public class Order : BaseEntity
{
    [Required, MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Confirmed, Processing, Shipped, Delivered, Cancelled

    public decimal SubTotal { get; set; }

    public decimal TaxAmount { get; set; } = 0;

    public decimal DiscountAmount { get; set; } = 0;

    public decimal ShippingCost { get; set; } = 0;

    public decimal TotalAmount { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    [MaxLength(200)]
    public string? ShippingAddress { get; set; }

    // Foreign Keys
    public Guid CustomerId { get; set; }

    // Navigation Properties
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
