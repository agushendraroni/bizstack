using Microsoft.AspNetCore.Mvc;
using TransactionService.DTOs;
using TransactionService.Services;

namespace TransactionService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var result = await _orderService.GetAllOrdersAsync();
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var result = await _orderService.GetOrderByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetOrdersByCustomer(Guid customerId)
    {
        var result = await _orderService.GetOrdersByCustomerAsync(customerId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetOrdersByStatus(string status)
    {
        var result = await _orderService.GetOrdersByStatusAsync(status);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        var result = await _orderService.CreateOrderAsync(createOrderDto);
        return result.IsSuccess ? CreatedAtAction(nameof(GetOrderById), new { id = result.Data?.Id }, result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderDto updateOrderDto)
    {
        var result = await _orderService.UpdateOrderAsync(id, updateOrderDto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        var result = await _orderService.CancelOrderAsync(id);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id}/confirm")]
    public async Task<IActionResult> ConfirmOrder(Guid id)
    {
        var result = await _orderService.ConfirmOrderAsync(id);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
