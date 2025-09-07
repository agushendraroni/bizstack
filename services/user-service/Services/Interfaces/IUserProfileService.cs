using SharedLibrary.DTOs;
using UserService.DTOs.UserProfile;


namespace UserService.Services.Interfaces;
public interface IUserProfileService
{
    Task<UserProfileResponse> CreateAsync(CreateUserProfileRequest request, int? tenantId = null, Guid? userId = null);
    Task<UserProfileResponse> UpdateAsync(Guid id, UpdateUserProfileRequest request);
    Task<bool> DeleteAsync(Guid id, int? tenantId = null, Guid? userId = null);
    Task<UserProfileResponse> GetByIdAsync(Guid id);
    Task<PaginatedResponse<UserProfileResponse>> GetFilteredAsync(UserProfileFilterRequest filter);
}