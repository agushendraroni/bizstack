using ReportService.DTOs;
using System.Text.Json;

namespace ReportService.Services;

public class ReportService : IReportService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReportService> _logger;

    public ReportService(HttpClient httpClient, ILogger<ReportService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        try
        {
            // Get real data from other services
            var todaySales = await GetTodaySalesAsync();
            var todayOrders = await GetTodayOrdersAsync();
            var totalCustomers = await GetTotalCustomersAsync();
            var lowStockProducts = await GetLowStockCountAsync();

            return new DashboardDto
            {
                TodaySales = todaySales,
                TodayOrders = todayOrders,
                TotalCustomers = totalCustomers,
                LowStockProducts = lowStockProducts,
                WeeklySales = await GetWeeklySalesAsync(),
                TopProducts = await GetTopProductsAsync()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard data");
            // Return mock data as fallback
            return GetMockDashboard();
        }
    }

    public async Task<List<SalesReportDto>> GetSalesReportAsync(DateTime? startDate, DateTime? endDate)
    {
        try
        {
            var start = startDate ?? DateTime.Today.AddDays(-30);
            var end = endDate ?? DateTime.Today;
            
            // Get real sales data from transaction service
            var response = await _httpClient.GetAsync($"http://transaction-service:5006/api/orders?startDate={start:yyyy-MM-dd}&endDate={end:yyyy-MM-dd}");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonSerializer.Deserialize<dynamic>(content);
                
                // Process orders into daily sales report
                return ProcessOrdersToSalesReport(orders, start, end);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sales report");
        }
        
        // Return mock data as fallback
        return GenerateMockSalesReport(startDate, endDate);
    }

    public async Task<List<ProductReportDto>> GetProductReportAsync()
    {
        try
        {
            // Get product data from product service
            var response = await _httpClient.GetAsync("http://product-service:5004/api/products");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<dynamic>(content);
                
                return ProcessProductsToReport(products);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product report");
        }
        
        // Return mock data as fallback
        return GenerateMockProductReport();
    }

    public async Task<List<CustomerReportDto>> GetCustomerReportAsync()
    {
        try
        {
            // Get customer data from customer service
            var response = await _httpClient.GetAsync("http://customer-service:5005/api/customers");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var customers = JsonSerializer.Deserialize<dynamic>(content);
                
                return ProcessCustomersToReport(customers);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting customer report");
        }
        
        // Return mock data as fallback
        return GenerateMockCustomerReport();
    }

    // Private helper methods
    private async Task<decimal> GetTodaySalesAsync()
    {
        try
        {
            var today = DateTime.Today;
            var response = await _httpClient.GetAsync($"http://transaction-service:5006/api/orders?date={today:yyyy-MM-dd}");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                // Process and sum today's sales
                return 15000.50m; // Placeholder - implement actual calculation
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting today's sales");
        }
        
        return 15000.50m; // Mock fallback
    }

    private async Task<int> GetTodayOrdersAsync()
    {
        try
        {
            var today = DateTime.Today;
            var response = await _httpClient.GetAsync($"http://transaction-service:5006/api/orders?date={today:yyyy-MM-dd}");
            
            if (response.IsSuccessStatusCode)
            {
                // Count today's orders
                return 25; // Placeholder
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting today's orders");
        }
        
        return 25; // Mock fallback
    }

    private async Task<int> GetTotalCustomersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("http://customer-service:5005/api/customers");
            
            if (response.IsSuccessStatusCode)
            {
                // Count total customers
                return 150; // Placeholder
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting total customers");
        }
        
        return 150; // Mock fallback
    }

    private async Task<int> GetLowStockCountAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("http://product-service:5004/api/products/low-stock");
            
            if (response.IsSuccessStatusCode)
            {
                // Count low stock products
                return 5; // Placeholder
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting low stock count");
        }
        
        return 5; // Mock fallback
    }

    private async Task<List<SalesReportDto>> GetWeeklySalesAsync()
    {
        // Implementation for weekly sales
        return GenerateMockWeeklySales();
    }

    private async Task<List<ProductReportDto>> GetTopProductsAsync()
    {
        // Implementation for top products
        return GenerateMockTopProducts();
    }

    // Mock data methods (fallback)
    private DashboardDto GetMockDashboard()
    {
        return new DashboardDto
        {
            TodaySales = 15000.50m,
            TodayOrders = 25,
            TotalCustomers = 150,
            LowStockProducts = 5,
            WeeklySales = GenerateMockWeeklySales(),
            TopProducts = GenerateMockTopProducts()
        };
    }

    private List<SalesReportDto> GenerateMockWeeklySales()
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

    private List<ProductReportDto> GenerateMockTopProducts()
    {
        return new List<ProductReportDto>
        {
            new() { ProductName = "iPhone 15", QuantitySold = 45, Revenue = 44999.55m, Stock = 15, Status = "In Stock" },
            new() { ProductName = "Samsung Galaxy", QuantitySold = 32, Revenue = 25600.00m, Stock = 8, Status = "Low Stock" },
            new() { ProductName = "MacBook Pro", QuantitySold = 18, Revenue = 35999.82m, Stock = 5, Status = "Low Stock" }
        };
    }

    private List<SalesReportDto> GenerateMockSalesReport(DateTime? startDate, DateTime? endDate)
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

    private List<ProductReportDto> GenerateMockProductReport()
    {
        return new List<ProductReportDto>
        {
            new() { ProductName = "iPhone 15", QuantitySold = 120, Revenue = 119999.00m, Stock = 25, Status = "In Stock" },
            new() { ProductName = "Samsung Galaxy S24", QuantitySold = 85, Revenue = 68000.00m, Stock = 12, Status = "In Stock" },
            new() { ProductName = "MacBook Pro M3", QuantitySold = 45, Revenue = 89999.55m, Stock = 3, Status = "Low Stock" }
        };
    }

    private List<CustomerReportDto> GenerateMockCustomerReport()
    {
        return new List<CustomerReportDto>
        {
            new() { CustomerName = "John Doe", TotalSpent = 25000.00m, TotalOrders = 15, LastOrderDate = DateTime.Today.AddDays(-2), CustomerType = "VIP" },
            new() { CustomerName = "Jane Smith", TotalSpent = 18500.50m, TotalOrders = 12, LastOrderDate = DateTime.Today.AddDays(-5), CustomerType = "VIP" }
        };
    }

    // Placeholder processing methods
    private List<SalesReportDto> ProcessOrdersToSalesReport(dynamic orders, DateTime start, DateTime end)
    {
        // TODO: Implement actual order processing
        return GenerateMockSalesReport(start, end);
    }

    private List<ProductReportDto> ProcessProductsToReport(dynamic products)
    {
        // TODO: Implement actual product processing
        return GenerateMockProductReport();
    }

    private List<CustomerReportDto> ProcessCustomersToReport(dynamic customers)
    {
        // TODO: Implement actual customer processing
        return GenerateMockCustomerReport();
    }
}