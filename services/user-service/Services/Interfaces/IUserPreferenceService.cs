using SharedLibrary.DTOs;
using UserService.DTOs.UserPreference;

namespace UserService.Services.Interfaces;

public interface IUserPreferenceService
{
    Task<UserPreferenceResponse> CreateAsync(CreateUserPreferenceRequest request, string currentUser, int? tenantId = null, Guid? userId = null);
    Task<UserPreferenceResponse> UpdateAsync(Guid id, UpdateUserPreferenceRequest request, string currentUser);
    Task<UserPreferenceResponse?> GetByIdAsync(Guid id);
    Task<PaginatedResponse<UserPreferenceResponse>> GetFilteredAsync(UserPreferenceFilterRequest filter);
    Task<bool> DeleteAsync(Guid id, int? tenantId = null, Guid? userId = null);
}
