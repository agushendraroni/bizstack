using ReportService.DTOs;

namespace ReportService.Services;

public interface IReportService
{
    Task<DashboardDto> GetDashboardAsync();
    Task<List<SalesReportDto>> GetSalesReportAsync(DateTime? startDate, DateTime? endDate);
    Task<List<ProductReportDto>> GetProductReportAsync();
    Task<List<CustomerReportDto>> GetCustomerReportAsync();
}