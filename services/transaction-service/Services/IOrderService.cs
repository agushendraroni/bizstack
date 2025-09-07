using TransactionService.DTOs;
using SharedLibrary.DTOs;

namespace TransactionService.Services;

public interface IOrderService
{
    Task<ApiResponse<List<OrderDto>>> GetAllOrdersAsync(int? tenantId = null);
    Task<ApiResponse<OrderDto>> GetOrderByIdAsync(Guid id);
    Task<ApiResponse<List<OrderDto>>> GetOrdersByCustomerAsync(Guid customerId);
    Task<ApiResponse<OrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto, int? tenantId = null);
    Task<ApiResponse<OrderDto>> UpdateOrderAsync(Guid id, UpdateOrderDto updateOrderDto);
    Task<ApiResponse<string>> DeleteOrderAsync(Guid id);
    Task<ApiResponse<string>> UpdateOrderStatusAsync(Guid id, string status);
    Task<ApiResponse<List<OrderDto>>> GetOrdersByStatusAsync(string status);
    Task<ApiResponse<string>> CancelOrderAsync(Guid id);
    Task<ApiResponse<string>> ConfirmOrderAsync(Guid id);
}