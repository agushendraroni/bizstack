using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.DTOs;
using NotificationService.Models;
using SharedLibrary.DTOs;

namespace NotificationService.Services;

public class NotificationService : INotificationService
{
    private readonly NotificationDbContext _context;

    public NotificationService(NotificationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<NotificationDto>> SendNotificationAsync(NotificationDto dto)
    {
        var notification = new Notification
        {
            Type = dto.Type,
            Recipient = dto.Recipient,
            Subject = dto.Subject,
            Message = dto.Message,
            Status = "Pending",
            RelatedEntityId = null,
            RelatedEntityType = null
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        var result = new NotificationDto
        {
            Id = notification.Id,
            Type = notification.Type,
            Recipient = notification.Recipient,
            Subject = notification.Subject,
            Message = notification.Message,
            Status = notification.Status,
            CreatedAt = notification.CreatedAt
        };

        return new ApiResponse<NotificationDto>
        {
            Data = result,
            IsSuccess = true,
            Message = "Notification sent successfully"
        };
    }

    public async Task<ApiResponse<List<NotificationDto>>> GetNotificationsAsync(Guid? userId = null)
    {
        var notifications = await _context.Notifications
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        var result = notifications.Select(n => new NotificationDto
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
        }).ToList();

        return new ApiResponse<List<NotificationDto>>
        {
            Data = result,
            IsSuccess = true,
            Message = "Notifications retrieved successfully"
        };
    }

    public async Task<ApiResponse<NotificationDto>> GetNotificationByIdAsync(Guid id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        
        if (notification == null)
        {
            return new ApiResponse<NotificationDto>
            {
                Data = null,
                IsSuccess = false,
                Message = "Notification not found"
            };
        }

        var result = new NotificationDto
        {
            Id = notification.Id,
            Type = notification.Type,
            Recipient = notification.Recipient,
            Subject = notification.Subject,
            Message = notification.Message,
            Status = notification.Status,
            SentAt = notification.SentAt,
            ErrorMessage = notification.ErrorMessage,
            RetryCount = notification.RetryCount,
            CreatedAt = notification.CreatedAt
        };

        return new ApiResponse<NotificationDto>
        {
            Data = result,
            IsSuccess = true,
            Message = "Notification retrieved successfully"
        };
    }

    public async Task<ApiResponse<string>> SendEmailAsync(SendEmailDto dto)
    {
        // Simplified email sending
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

        return new ApiResponse<string>
        {
            Data = "Email sent",
            IsSuccess = true,
            Message = "Email sent successfully"
        };
    }

    public async Task<ApiResponse<string>> SendSmsAsync(SendSmsDto dto)
    {
        // Simplified SMS sending
        var notification = new Notification
        {
            Type = "SMS",
            Recipient = dto.PhoneNumber,
            Subject = "SMS",
            Message = dto.Message,
            Status = "Sent",
            SentAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return new ApiResponse<string>
        {
            Data = "SMS sent",
            IsSuccess = true,
            Message = "SMS sent successfully"
        };
    }

    public async Task<ApiResponse<string>> SendWhatsAppAsync(SendWhatsAppDto dto)
    {
        // Simplified WhatsApp sending
        var notification = new Notification
        {
            Type = "WhatsApp",
            Recipient = dto.PhoneNumber,
            Subject = "WhatsApp",
            Message = dto.Message,
            Status = "Sent",
            SentAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return new ApiResponse<string>
        {
            Data = "WhatsApp sent",
            IsSuccess = true,
            Message = "WhatsApp sent successfully"
        };
    }

    public async Task<ApiResponse<string>> MarkAsReadAsync(Guid id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        
        if (notification == null)
        {
            return new ApiResponse<string>
            {
                Data = null,
                IsSuccess = false,
                Message = "Notification not found"
            };
        }

        notification.Status = "Read";
        await _context.SaveChangesAsync();

        return new ApiResponse<string>
        {
            Data = "Marked as read",
            IsSuccess = true,
            Message = "Notification marked as read"
        };
    }

    public async Task<ApiResponse<string>> DeleteNotificationAsync(Guid id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        
        if (notification == null)
        {
            return new ApiResponse<string>
            {
                Data = null,
                IsSuccess = false,
                Message = "Notification not found"
            };
        }

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();

        return new ApiResponse<string>
        {
            Data = "Deleted",
            IsSuccess = true,
            Message = "Notification deleted successfully"
        };
    }
}