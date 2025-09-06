namespace ReportService.DTOs;

public class SalesReportDto
{
    public DateTime Date { get; set; }
    public decimal TotalSales { get; set; }
    public int TotalOrders { get; set; }
    public decimal AverageOrderValue { get; set; }
    public int TotalCustomers { get; set; }
}

public class ProductReportDto
{
    public string ProductName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
    public int Stock { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CustomerReportDto
{
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalSpent { get; set; }
    public int TotalOrders { get; set; }
    public DateTime LastOrderDate { get; set; }
    public string CustomerType { get; set; } = string.Empty;
}

public class DashboardDto
{
    public decimal TodaySales { get; set; }
    public int TodayOrders { get; set; }
    public int TotalCustomers { get; set; }
    public int LowStockProducts { get; set; }
    public List<SalesReportDto> WeeklySales { get; set; } = new();
    public List<ProductReportDto> TopProducts { get; set; } = new();
}
