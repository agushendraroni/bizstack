using CustomerService.DTOs;
using SharedLibrary.DTOs;

namespace CustomerService.Services;

public interface ICustomerService
{
    Task<ApiResponse<List<CustomerDto>>> GetAllCustomersAsync(int? tenantId = null);
    Task<ApiResponse<CustomerDto>> GetCustomerByIdAsync(Guid id);
    Task<ApiResponse<List<CustomerDto>>> GetCustomersByGroupAsync(Guid groupId);
    Task<ApiResponse<List<CustomerDto>>> SearchCustomersAsync(string searchTerm);
    Task<ApiResponse<List<CustomerDto>>> GetVipCustomersAsync();
    Task<ApiResponse<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto, int? tenantId = null);
    Task<ApiResponse<CustomerDto>> UpdateCustomerAsync(Guid id, UpdateCustomerDto updateCustomerDto);
    Task<ApiResponse<string>> DeleteCustomerAsync(Guid id);
    Task<ApiResponse<string>> UpdateCustomerStatsAsync(Guid id, decimal orderAmount);
}