using Users.Models;

namespace Users.Services
{
    public interface IUsersService
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<List<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(Guid id, User updatedUser);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
