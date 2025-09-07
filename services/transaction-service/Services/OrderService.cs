using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TransactionService.Data;
using TransactionService.DTOs;
using TransactionService.Models;
using SharedLibrary.DTOs;

namespace TransactionService.Services;

public class OrderService : IOrderService
{
    private readonly TransactionDbContext _context;
    private readonly IMapper _mapper;

    public OrderService(TransactionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<OrderDto>>> GetAllOrdersAsync(int? tenantId = null)
    {
        var orders = await _context.Orders.Where(x => !x.IsDeleted)
            .Include(o => o.OrderItems)
            .Include(o => o.Payments)
            .Where(o => o.IsActive)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        var orderDtos = _mapper.Map<List<OrderDto>>(orders);
        return ApiResponse<List<OrderDto>>.Success(orderDtos);
    }

    public async Task<ApiResponse<OrderDto>> GetOrderByIdAsync(Guid id)
    {
        var order = await _context.Orders.Where(x => !x.IsDeleted)
            .Include(o => o.OrderItems)
            .Include(o => o.Payments)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return ApiResponse<OrderDto>.Error("Order not found");

        var orderDto = _mapper.Map<OrderDto>(order);
        return ApiResponse<OrderDto>.Success(orderDto);
    }

    public async Task<ApiResponse<List<OrderDto>>> GetOrdersByCustomerAsync(Guid customerId)
    {
        var orders = await _context.Orders.Where(x => !x.IsDeleted)
            .Include(o => o.OrderItems)
            .Include(o => o.Payments)
            .Where(o => o.CustomerId == customerId && o.IsActive)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        var orderDtos = _mapper.Map<List<OrderDto>>(orders);
        return ApiResponse<List<OrderDto>>.Success(orderDtos);
    }

    public async Task<ApiResponse<List<OrderDto>>> GetOrdersByStatusAsync(string status)
    {
        var orders = await _context.Orders.Where(x => !x.IsDeleted)
            .Include(o => o.OrderItems)
            .Include(o => o.Payments)
            .Where(o => o.Status == status && o.IsActive)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        var orderDtos = _mapper.Map<List<OrderDto>>(orders);
        return ApiResponse<List<OrderDto>>.Success(orderDtos);
    }

    public async Task<ApiResponse<OrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto, int? tenantId = null)
    {
        var order = new Order
        {
            OrderNumber = GenerateOrderNumber(),
            CustomerId = createOrderDto.CustomerId,
            Notes = createOrderDto.Notes,
            ShippingAddress = createOrderDto.ShippingAddress,
            TaxAmount = createOrderDto.TaxAmount,
            DiscountAmount = createOrderDto.DiscountAmount,
            ShippingCost = createOrderDto.ShippingCost
        };

        // Add order items
        foreach (var itemDto in createOrderDto.OrderItems)
        {
            var orderItem = new OrderItem
            {
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice,
                DiscountAmount = itemDto.DiscountAmount,
                TotalPrice = (itemDto.UnitPrice * itemDto.Quantity) - itemDto.DiscountAmount,
                ProductName = itemDto.ProductName,
                ProductCode = itemDto.ProductCode,
                Unit = itemDto.Unit
            };
            order.OrderItems.Add(orderItem);
        }

        // Calculate totals
        order.SubTotal = order.OrderItems.Sum(oi => oi.TotalPrice);
        order.TotalAmount = order.SubTotal + order.TaxAmount - order.DiscountAmount + order.ShippingCost;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var orderDto = _mapper.Map<OrderDto>(order);
        return ApiResponse<OrderDto>.Success(orderDto);
    }

    public async Task<ApiResponse<OrderDto>> UpdateOrderAsync(Guid id, UpdateOrderDto updateOrderDto)
    {
        var order = await _context.Orders.Where(x => !x.IsDeleted)
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return ApiResponse<OrderDto>.Error("Order not found");

        if (!string.IsNullOrEmpty(updateOrderDto.Status)) order.Status = updateOrderDto.Status;
        if (updateOrderDto.Notes != null) order.Notes = updateOrderDto.Notes;
        if (updateOrderDto.ShippingAddress != null) order.ShippingAddress = updateOrderDto.ShippingAddress;
        if (updateOrderDto.TaxAmount.HasValue) order.TaxAmount = updateOrderDto.TaxAmount.Value;
        if (updateOrderDto.DiscountAmount.HasValue) order.DiscountAmount = updateOrderDto.DiscountAmount.Value;
        if (updateOrderDto.ShippingCost.HasValue) order.ShippingCost = updateOrderDto.ShippingCost.Value;

        // Recalculate total if amounts changed
        if (updateOrderDto.TaxAmount.HasValue || updateOrderDto.DiscountAmount.HasValue || updateOrderDto.ShippingCost.HasValue)
        {
            order.TotalAmount = order.SubTotal + order.TaxAmount - order.DiscountAmount + order.ShippingCost;
        }

        await _context.SaveChangesAsync();

        var orderDto = _mapper.Map<OrderDto>(order);
        return ApiResponse<OrderDto>.Success(orderDto);
    }

    public async Task<ApiResponse<string>> CancelOrderAsync(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return ApiResponse<string>.Error("Order not found");

        if (order.Status == "Delivered" || order.Status == "Shipped")
            return ApiResponse<string>.Error("Cannot cancel delivered or shipped order");

        order.Status = "Cancelled";
        await _context.SaveChangesAsync();

        return ApiResponse<string>.Success("Order cancelled successfully");
    }

    public async Task<ApiResponse<string>> ConfirmOrderAsync(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return ApiResponse<string>.Error("Order not found");

        if (order.Status != "Pending")
            return ApiResponse<string>.Error("Only pending orders can be confirmed");

        order.Status = "Confirmed";
        await _context.SaveChangesAsync();

        return ApiResponse<string>.Success("Order confirmed successfully");
    }

    public async Task<ApiResponse<string>> DeleteOrderAsync(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return new ApiResponse<string> { Data = null, IsSuccess = false, Message = "Order not found" };

        order.IsActive = false;
        await _context.SaveChangesAsync();

        return new ApiResponse<string> { Data = "Order deleted successfully", IsSuccess = true, Message = "Order deleted successfully" };
    }

    public async Task<ApiResponse<string>> UpdateOrderStatusAsync(Guid id, string status)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return new ApiResponse<string> { Data = null, IsSuccess = false, Message = "Order not found" };

        order.Status = status;
        await _context.SaveChangesAsync();

        return new ApiResponse<string> { Data = "Order status updated successfully", IsSuccess = true, Message = "Order status updated successfully" };
    }

    private string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
    }
}
