namespace CustomerService.DTOs;

public class CustomerGroupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal? MinimumSpent { get; set; }
    public string? Color { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CustomerCount { get; set; }
    public bool IsActive { get; set; }
}

public class CreateCustomerGroupDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? DiscountPercentage { get; set; } = 0;
    public decimal? MinimumSpent { get; set; } = 0;
    public string? Color { get; set; }
}

public class UpdateCustomerGroupDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal? MinimumSpent { get; set; }
    public string? Color { get; set; }
}
