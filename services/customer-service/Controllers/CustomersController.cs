using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using CustomerService.DTOs;
using CustomerService.Services;

namespace CustomerService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var result = await _customerService.GetAllCustomersAsync(GetTenantId());
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
        var result = await _customerService.GetCustomerByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("group/{groupId}")]
    public async Task<IActionResult> GetCustomersByGroup(Guid groupId)
    {
        var result = await _customerService.GetCustomersByGroupAsync(groupId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("search/{searchTerm}")]
    public async Task<IActionResult> SearchCustomers(string searchTerm)
    {
        var result = await _customerService.SearchCustomersAsync(searchTerm);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("vip")]
    public async Task<IActionResult> GetVipCustomers()
    {
        var result = await _customerService.GetVipCustomersAsync();
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto createCustomerDto)
    {
        var result = await _customerService.CreateCustomerAsync(createCustomerDto, GetTenantId());
        return result.IsSuccess ? CreatedAtAction(nameof(GetCustomerById), new { id = result.Data?.Id }, result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomerDto updateCustomerDto)
    {
        var result = await _customerService.UpdateCustomerAsync(id, updateCustomerDto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        var result = await _customerService.DeleteCustomerAsync(id);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id}/stats")]
    public async Task<IActionResult> UpdateCustomerStats(Guid id, [FromBody] UpdateStatsDto updateStatsDto)
    {
        var result = await _customerService.UpdateCustomerStatsAsync(id, updateStatsDto.OrderAmount);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    private int? GetTenantId()
    {
        if (Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdHeader) && 
            int.TryParse(tenantIdHeader.FirstOrDefault(), out var tenantId))
        {
            return tenantId;
        }
        return null;
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

public class UpdateStatsDto
{
    public decimal OrderAmount { get; set; }
}
