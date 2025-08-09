using SharedLibrary.DTOs;
using UserService.DTOs.UserActivityLog;

namespace UserService.Services.Interfaces;

public interface IUserActivityLogService
{
    Task<UserActivityLogResponse> CreateAsync(CreateUserActivityLogRequest request, string currentUser);
    Task<UserActivityLogResponse?> GetByIdAsync(Guid id);
    Task<PaginatedResponse<UserActivityLogResponse>> GetFilteredAsync(UserActivityLogFilterRequest filter);
}
