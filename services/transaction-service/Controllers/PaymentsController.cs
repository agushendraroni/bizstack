using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using TransactionService.Data;
using TransactionService.DTOs;
using TransactionService.Models;
using SharedLibrary.DTOs;

namespace TransactionService.Controllers;

[ApiController]
[ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly TransactionDbContext _context;

    public PaymentsController(TransactionDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetPayments()
    {
        var payments = await _context.Payments
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.PaymentDate)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                PaymentNumber = p.PaymentNumber,
                PaymentDate = p.PaymentDate,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                Status = p.Status,
                ReferenceNumber = p.ReferenceNumber,
                Notes = p.Notes,
                OrderId = p.OrderId
            })
            .ToListAsync();

        return Ok(ApiResponse<List<PaymentDto>>.Success(payments));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPayment(Guid id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null)
            return NotFound(ApiResponse<PaymentDto>.Error("Payment not found"));

        var paymentDto = new PaymentDto
        {
            Id = payment.Id,
            PaymentNumber = payment.PaymentNumber,
            PaymentDate = payment.PaymentDate,
            Amount = payment.Amount,
            PaymentMethod = payment.PaymentMethod,
            Status = payment.Status,
            ReferenceNumber = payment.ReferenceNumber,
            Notes = payment.Notes,
            OrderId = payment.OrderId
        };

        return Ok(ApiResponse<PaymentDto>.Success(paymentDto));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto)
    {
        var payment = new Payment
        {
            PaymentNumber = GeneratePaymentNumber(),
            OrderId = dto.OrderId,
            Amount = dto.Amount,
            PaymentMethod = dto.PaymentMethod,
            ReferenceNumber = dto.ReferenceNumber,
            Notes = dto.Notes,
            Status = "Completed"
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        var paymentDto = new PaymentDto
        {
            Id = payment.Id,
            PaymentNumber = payment.PaymentNumber,
            PaymentDate = payment.PaymentDate,
            Amount = payment.Amount,
            PaymentMethod = payment.PaymentMethod,
            Status = payment.Status,
            ReferenceNumber = payment.ReferenceNumber,
            Notes = payment.Notes,
            OrderId = payment.OrderId
        };

        return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, ApiResponse<PaymentDto>.Success(paymentDto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePayment(Guid id, [FromBody] UpdatePaymentDto dto)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null)
            return NotFound(ApiResponse<PaymentDto>.Error("Payment not found"));

        if (!string.IsNullOrEmpty(dto.Status)) payment.Status = dto.Status;
        if (dto.ReferenceNumber != null) payment.ReferenceNumber = dto.ReferenceNumber;
        if (dto.Notes != null) payment.Notes = dto.Notes;

        await _context.SaveChangesAsync();

        var paymentDto = new PaymentDto
        {
            Id = payment.Id,
            PaymentNumber = payment.PaymentNumber,
            PaymentDate = payment.PaymentDate,
            Amount = payment.Amount,
            PaymentMethod = payment.PaymentMethod,
            Status = payment.Status,
            ReferenceNumber = payment.ReferenceNumber,
            Notes = payment.Notes,
            OrderId = payment.OrderId
        };

        return Ok(ApiResponse<PaymentDto>.Success(paymentDto));
    }

    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetPaymentsByOrder(Guid orderId)
    {
        var payments = await _context.Payments
            .Where(p => p.OrderId == orderId && p.IsActive)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                PaymentNumber = p.PaymentNumber,
                PaymentDate = p.PaymentDate,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                Status = p.Status,
                ReferenceNumber = p.ReferenceNumber,
                Notes = p.Notes,
                OrderId = p.OrderId
            })
            .ToListAsync();

        return Ok(ApiResponse<List<PaymentDto>>.Success(payments));
    }

    private string GeneratePaymentNumber()
    {
        return $"PAY-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
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
