using SharedLibrary.DTOs;
using UserService.DTOs.UserProfile;


namespace UserService.Services.Interfaces;
public interface IUserProfileService
{
    Task<UserProfileResponse> CreateAsync(CreateUserProfileRequest request);
    Task<UserProfileResponse> UpdateAsync(Guid id, UpdateUserProfileRequest request);
    Task<bool> DeleteAsync(Guid id);
    Task<UserProfileResponse> GetByIdAsync(Guid id);
    Task<PaginatedResponse<UserProfileResponse>> GetFilteredAsync(UserProfileFilterRequest filter);
}