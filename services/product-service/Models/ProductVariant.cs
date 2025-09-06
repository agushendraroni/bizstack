using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace ProductService.Models;

public class ProductVariant : BaseEntity
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty; // Size, Color, etc.

    [MaxLength(100)]
    public string Value { get; set; } = string.Empty; // Large, Red, etc.

    public decimal? PriceAdjustment { get; set; } = 0; // +/- from base price

    public int Stock { get; set; } = 0;

    [MaxLength(50)]
    public string? Sku { get; set; } // Stock Keeping Unit

    // Foreign Keys
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
