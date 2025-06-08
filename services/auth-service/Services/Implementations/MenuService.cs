using AuthService.Data;
using AuthService.DTOs.Menu;
using SharedLibrary.DTOs;
using AuthService.Interfaces;
using AuthService.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AuthService.Services.Interfaces;
using FluentValidation;

namespace AuthService.Services.Implementations
{
    public class MenuService : IMenuService
    {
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateMenuRequest> _createValidator;
        private readonly IValidator<UpdateMenuRequest> _updateValidator;

        public MenuService(
            AuthDbContext context,
            IMapper mapper,
            IValidator<CreateMenuRequest> createValidator,
            IValidator<UpdateMenuRequest> updateValidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<MenuResponse> CreateAsync(CreateMenuRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var menu = _mapper.Map<Menu>(request);
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
            return _mapper.Map<MenuResponse>(menu);
        }

        public async Task<MenuResponse> UpdateAsync(int id, UpdateMenuRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var menu = await _context.Menus.FindAsync(id)
                ?? throw new KeyNotFoundException("Menu not found");

            _mapper.Map(request, menu);
            menu.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return _mapper.Map<MenuResponse>(menu);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var menu = await _context.Menus.FindAsync(id)
                ?? throw new KeyNotFoundException("Menu not found");

            menu.IsDeleted = true;
            menu.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<MenuResponse> GetByIdAsync(int id)
        {
            var menu = await _context.Menus.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted)
                ?? throw new KeyNotFoundException("Menu not found");

            return _mapper.Map<MenuResponse>(menu);
        }

        public async Task<PaginatedResponse<MenuResponse>> GetAllAsync(MenuFilterRequest filter)
        {
            var query = _context.Menus.AsQueryable().Where(m => !m.IsDeleted);

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(m => m.Name.Contains(filter.Name));

            if (filter.CompanyId.HasValue)
                query = query.Where(m => m.CompanyId == filter.CompanyId);

            var totalCount = await query.CountAsync();
            var data = await query
                .OrderBy(m => m.OrderIndex)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result = _mapper.Map<List<MenuResponse>>(data);
            return new PaginatedResponse<MenuResponse>(result, totalCount, filter.Page, filter.PageSize);
        }
    }
}