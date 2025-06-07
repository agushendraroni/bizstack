using AuthService.DTOs.Menu;

namespace AuthService.Services.Interfaces
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuResponse>> GetAllAsync(MenuFilterRequest filter);
        Task<MenuResponse?> GetByIdAsync(int id);
        Task<MenuResponse> CreateAsync(CreateMenuRequest request);
        Task<MenuResponse?> UpdateAsync(int id, UpdateMenuRequest request);
        Task<bool> DeleteAsync(int id);
    }
}