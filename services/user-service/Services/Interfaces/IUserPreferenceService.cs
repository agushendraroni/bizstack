using SharedLibrary.DTOs;
using UserService.DTOs.UserPreference;

namespace UserService.Services.Interfaces;

public interface IUserPreferenceService
{
    Task<UserPreferenceResponse> CreateAsync(CreateUserPreferenceRequest request, string currentUser);
    Task<UserPreferenceResponse> UpdateAsync(Guid id, UpdateUserPreferenceRequest request, string currentUser);
    Task<UserPreferenceResponse?> GetByIdAsync(Guid id);
    Task<PaginatedResponse<UserPreferenceResponse>> GetFilteredAsync(UserPreferenceFilterRequest filter);
    Task<bool> DeleteAsync(Guid id);
}
