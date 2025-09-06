using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace ProductService.Models;

public class Category : BaseEntity
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(200)]
    public string? ImageUrl { get; set; }

    // Self-referencing for parent/child categories
    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }

    // Navigation Properties
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
