using NotificationService.DTOs;
using SharedLibrary.DTOs;

namespace NotificationService.Services;

public interface INotificationService
{
    Task<ApiResponse<NotificationDto>> SendNotificationAsync(NotificationDto notification);
    Task<ApiResponse<List<NotificationDto>>> GetNotificationsAsync(Guid? userId = null);
    Task<ApiResponse<NotificationDto>> GetNotificationByIdAsync(Guid id);
    Task<ApiResponse<string>> MarkAsReadAsync(Guid id);
    Task<ApiResponse<string>> DeleteNotificationAsync(Guid id);
}