using TransactionService.DTOs;
using SharedLibrary.DTOs;

namespace TransactionService.Services;

public interface IOrderService
{
    Task<ApiResponse<List<OrderDto>>> GetAllOrdersAsync();
    Task<ApiResponse<OrderDto>> GetOrderByIdAsync(Guid id);
    Task<ApiResponse<List<OrderDto>>> GetOrdersByCustomerAsync(Guid customerId);
    Task<ApiResponse<List<OrderDto>>> GetOrdersByStatusAsync(string status);
    Task<ApiResponse<OrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto);
    Task<ApiResponse<OrderDto>> UpdateOrderAsync(Guid id, UpdateOrderDto updateOrderDto);
    Task<ApiResponse<string>> CancelOrderAsync(Guid id);
    Task<ApiResponse<string>> ConfirmOrderAsync(Guid id);
}
