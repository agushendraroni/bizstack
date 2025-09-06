using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace ProductService.Models;

public class Product : BaseEntity
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal? DiscountPrice { get; set; }

    public int Stock { get; set; } = 0;

    public int MinStock { get; set; } = 0;

    [MaxLength(50)]
    public string Unit { get; set; } = "pcs"; // pcs, kg, liter, etc.

    [MaxLength(200)]
    public string? ImageUrl { get; set; }

    // Foreign Keys
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    // Navigation Properties
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
}
