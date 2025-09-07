using SharedLibrary.DTOs;
using UserService.DTOs.UserActivityLog;

namespace UserService.Services.Interfaces;

public interface IUserActivityLogService
{
    Task<UserActivityLogResponse> CreateAsync(CreateUserActivityLogRequest request, string currentUser, int? tenantId = null, Guid? userId = null);
    Task<UserActivityLogResponse?> GetByIdAsync(Guid id);
    Task<PaginatedResponse<UserActivityLogResponse>> GetFilteredAsync(UserActivityLogFilterRequest filter);
}
