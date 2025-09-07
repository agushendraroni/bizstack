using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.DTOs;
using NotificationService.Models;
using SharedLibrary.DTOs;

namespace NotificationService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationDbContext _context;

    public NotificationsController(NotificationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        try
        {
            var notifications = await _context.Notifications
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Type = n.Type,
                    Recipient = n.Recipient,
                    Subject = n.Subject,
                    Message = n.Message,
                    Status = n.Status,
                    SentAt = n.SentAt,
                    ErrorMessage = n.ErrorMessage,
                    RetryCount = n.RetryCount,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();

            return Ok(ApiResponse<List<NotificationDto>>.Success(notifications));
        }
        catch
        {
            // Return empty list if database not ready
            return Ok(ApiResponse<List<NotificationDto>>.Success(new List<NotificationDto>()));
        }
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(ApiResponse<string>.Success("Notification service is working"));
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationDto dto)
    {
        var notification = new Notification
        {
            Type = dto.Type,
            Recipient = dto.Recipient,
            Subject = dto.Subject,
            Message = dto.Message,
            RelatedEntityId = dto.RelatedEntityId,
            RelatedEntityType = dto.RelatedEntityType,
            Status = "Sent",
            SentAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<string>.Success($"{dto.Type} notification sent to {dto.Recipient}"));
    }

    [HttpPost("email")]
    public async Task<IActionResult> SendEmail([FromBody] SendEmailDto dto)
    {
        var notification = new Notification
        {
            Type = "Email",
            Recipient = dto.To,
            Subject = dto.Subject,
            Message = dto.Body,
            Status = "Sent",
            SentAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<string>.Success($"Email sent to {dto.To}"));
    }

    [HttpPost("sms")]
    public async Task<IActionResult> SendSms([FromBody] SendSmsDto dto)
    {
        var notification = new Notification
        {
            Type = "SMS",
            Recipient = dto.PhoneNumber,
            Subject = "SMS Notification",
            Message = dto.Message,
            Status = "Sent",
            SentAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<string>.Success($"SMS sent to {dto.PhoneNumber}"));
    }

    [HttpPost("whatsapp")]
    public async Task<IActionResult> SendWhatsApp([FromBody] SendWhatsAppDto dto)
    {
        var notification = new Notification
        {
            Type = "WhatsApp",
            Recipient = dto.PhoneNumber,
            Subject = "WhatsApp Message",
            Message = dto.Message,
            Status = "Sent",
            SentAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<string>.Success($"WhatsApp message sent to {dto.PhoneNumber}"));
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetNotificationsByStatus(string status)
    {
        var notifications = await _context.Notifications
            .Where(n => n.Status == status)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Type = n.Type,
                Recipient = n.Recipient,
                Subject = n.Subject,
                Message = n.Message,
                Status = n.Status,
                SentAt = n.SentAt,
                ErrorMessage = n.ErrorMessage,
                RetryCount = n.RetryCount,
                CreatedAt = n.CreatedAt
            })
            .ToListAsync();

        return Ok(ApiResponse<List<NotificationDto>>.Success(notifications));
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
