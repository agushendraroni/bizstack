using SharedLibrary.DTOs;
using AuthService.DTOs.Menu;

namespace AuthService.Services.Interfaces
{
    public interface IMenuService
    {
        Task<MenuResponse> CreateAsync(CreateMenuRequest request);
        Task<MenuResponse> UpdateAsync(int id, UpdateMenuRequest request);
        Task<bool> DeleteAsync(int id);
        Task<MenuResponse> GetByIdAsync(int id);
        Task<PaginatedResponse<MenuResponse>> GetAllAsync(MenuFilterRequest filter);
    }
}