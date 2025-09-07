using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using ReportService.DTOs;
using SharedLibrary.DTOs;

namespace ReportService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ReportsController : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var dashboard = new DashboardDto
        {
            TodaySales = 15000.50m,
            TodayOrders = 25,
            TotalCustomers = 150,
            LowStockProducts = 5,
            WeeklySales = GenerateWeeklySales(),
            TopProducts = GenerateTopProducts()
        };

        return Ok(ApiResponse<DashboardDto>.Success(dashboard));
    }

    [HttpGet("sales")]
    public async Task<IActionResult> GetSalesReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var reports = GenerateSalesReport(startDate, endDate);
        return Ok(ApiResponse<List<SalesReportDto>>.Success(reports));
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProductReport()
    {
        var reports = GenerateProductReport();
        return Ok(ApiResponse<List<ProductReportDto>>.Success(reports));
    }

    [HttpGet("customers")]
    public async Task<IActionResult> GetCustomerReport()
    {
        var reports = GenerateCustomerReport();
        return Ok(ApiResponse<List<CustomerReportDto>>.Success(reports));
    }

    private List<SalesReportDto> GenerateWeeklySales()
    {
        var sales = new List<SalesReportDto>();
        for (int i = 6; i >= 0; i--)
        {
            var date = DateTime.Today.AddDays(-i);
            sales.Add(new SalesReportDto
            {
                Date = date,
                TotalSales = Random.Shared.Next(5000, 20000),
                TotalOrders = Random.Shared.Next(10, 50),
                AverageOrderValue = Random.Shared.Next(200, 800),
                TotalCustomers = Random.Shared.Next(5, 25)
            });
        }
        return sales;
    }

    private List<ProductReportDto> GenerateTopProducts()
    {
        return new List<ProductReportDto>
        {
            new() { ProductName = "iPhone 15", QuantitySold = 45, Revenue = 44999.55m, Stock = 15, Status = "In Stock" },
            new() { ProductName = "Samsung Galaxy", QuantitySold = 32, Revenue = 25600.00m, Stock = 8, Status = "Low Stock" },
            new() { ProductName = "MacBook Pro", QuantitySold = 18, Revenue = 35999.82m, Stock = 5, Status = "Low Stock" }
        };
    }

    private List<SalesReportDto> GenerateSalesReport(DateTime? startDate, DateTime? endDate)
    {
        var start = startDate ?? DateTime.Today.AddDays(-30);
        var end = endDate ?? DateTime.Today;
        var reports = new List<SalesReportDto>();

        for (var date = start; date <= end; date = date.AddDays(1))
        {
            reports.Add(new SalesReportDto
            {
                Date = date,
                TotalSales = Random.Shared.Next(3000, 25000),
                TotalOrders = Random.Shared.Next(5, 60),
                AverageOrderValue = Random.Shared.Next(150, 900),
                TotalCustomers = Random.Shared.Next(3, 30)
            });
        }
        return reports;
    }

    private List<ProductReportDto> GenerateProductReport()
    {
        return new List<ProductReportDto>
        {
            new() { ProductName = "iPhone 15", QuantitySold = 120, Revenue = 119999.00m, Stock = 25, Status = "In Stock" },
            new() { ProductName = "Samsung Galaxy S24", QuantitySold = 85, Revenue = 68000.00m, Stock = 12, Status = "In Stock" },
            new() { ProductName = "MacBook Pro M3", QuantitySold = 45, Revenue = 89999.55m, Stock = 3, Status = "Low Stock" },
            new() { ProductName = "iPad Air", QuantitySold = 67, Revenue = 40199.33m, Stock = 18, Status = "In Stock" },
            new() { ProductName = "AirPods Pro", QuantitySold = 156, Revenue = 46799.44m, Stock = 2, Status = "Critical" }
        };
    }

    private List<CustomerReportDto> GenerateCustomerReport()
    {
        return new List<CustomerReportDto>
        {
            new() { CustomerName = "John Doe", TotalSpent = 25000.00m, TotalOrders = 15, LastOrderDate = DateTime.Today.AddDays(-2), CustomerType = "VIP" },
            new() { CustomerName = "Jane Smith", TotalSpent = 18500.50m, TotalOrders = 12, LastOrderDate = DateTime.Today.AddDays(-5), CustomerType = "VIP" },
            new() { CustomerName = "Bob Johnson", TotalSpent = 8750.25m, TotalOrders = 8, LastOrderDate = DateTime.Today.AddDays(-10), CustomerType = "Regular" },
            new() { CustomerName = "Alice Brown", TotalSpent = 12300.75m, TotalOrders = 10, LastOrderDate = DateTime.Today.AddDays(-3), CustomerType = "Regular" }
        };
    }

    private Guid? GetUserId()
    {
        if (Request.Headers.TryGetValue("X-User-Id", out var userIdHeader) && 
            Guid.TryParse(userIdHeader.FirstOrDefault(), out var userId))
        {
            return userId;
        }
        return null;
    }
}
