using AuthService.Data;
using AuthService.DTOs.Menu;
using AuthService.Models;
using AuthService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Implementations
{
    public class MenuService : IMenuService
    {
        private readonly AuthDbContext _context;

        public MenuService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<MenuResponse> CreateAsync(CreateMenuRequest request)
        {
            var menu = new Menu
            {
                Name = request.Name,
                Url = request.Url,
                Icon = request.Icon,
                IsActive = request.IsActive,
                ParentId = request.ParentId,
                CompanyId = request.CompanyId
            };

            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();

            return MapToResponse(menu);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null) return false;

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<MenuResponse>> GetAllAsync(MenuFilterRequest filter)
        {
            var query = _context.Menus.AsQueryable();

            if (filter.CompanyId.HasValue)
                query = query.Where(m => m.CompanyId == filter.CompanyId);

            if (filter.IsActive.HasValue)
                query = query.Where(m => m.IsActive == filter.IsActive);

            var menus = await query.ToListAsync();
            return menus.Select(MapToResponse);
        }

        public async Task<MenuResponse?> GetByIdAsync(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            return menu == null ? null : MapToResponse(menu);
        }

        public async Task<MenuResponse?> UpdateAsync(int id, UpdateMenuRequest request)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null) return null;

            menu.Name = request.Name;
            menu.Url = request.Url;
            menu.Icon = request.Icon;
            menu.IsActive = request.IsActive;
            menu.ParentId = request.ParentId;

            await _context.SaveChangesAsync();
            return MapToResponse(menu);
        }

        private MenuResponse MapToResponse(Menu menu) => new()
        {
            Id = menu.Id,
            Name = menu.Name,
            Url = menu.Url ?? string.Empty,
            Icon = menu.Icon ?? string.Empty,
            IsActive = menu.IsActive,
            ParentId = menu.ParentId,
            CompanyId = menu.CompanyId
        };
    }
}