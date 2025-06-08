using AuthService.DTOs.UserPasswordHistory;

namespace AuthService.Services.Interfaces
{
 

     public interface IUserPasswordHistoryService
    {
        Task<UserPasswordHistoryResponse> CreateAsync(CreateUserPasswordHistoryRequest request);
        Task<UserPasswordHistoryResponse> UpdateAsync(int id, UpdateUserPasswordHistoryRequest request);
        Task<bool> DeleteAsync(int id);
        Task<UserPasswordHistoryResponse?> GetByIdAsync(int id);
        Task<IEnumerable<UserPasswordHistoryResponse>> GetAllAsync();
    }
}