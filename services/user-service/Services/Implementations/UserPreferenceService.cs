using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs;
using UserService.Data;
using UserService.DTOs.UserPreference;
using UserService.Models;
using UserService.Services.Interfaces;

namespace UserService.Services.Implementations;

public class UserPreferenceService : IUserPreferenceService
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;

    public UserPreferenceService(UserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserPreferenceResponse> CreateAsync(CreateUserPreferenceRequest request, string currentUser)
    {
        var entity = _mapper.Map<UserPreference>(request);
        entity.CreatedAt = DateTime.UtcNow;
        entity.CreatedBy = currentUser;
        entity.IsActive = true;

        _context.UserPreferences.Add(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserPreferenceResponse>(entity);
    }

    public async Task<UserPreferenceResponse> UpdateAsync(Guid id, UpdateUserPreferenceRequest request, string currentUser)
    {
        var entity = await _context.UserPreferences.FindAsync(id);
        if (entity == null) throw new KeyNotFoundException("UserPreference not found");

        _mapper.Map(request, entity);
        entity.ChangedAt = DateTime.UtcNow;
        entity.ChangedBy = currentUser;

        await _context.SaveChangesAsync();
        return _mapper.Map<UserPreferenceResponse>(entity);
    }

    public async Task<UserPreferenceResponse?> GetByIdAsync(Guid id)
    {
        var entity = await _context.UserPreferences.FindAsync(id);
        return entity == null ? null : _mapper.Map<UserPreferenceResponse>(entity);
    }

    public async Task<PaginatedResponse<UserPreferenceResponse>> GetFilteredAsync(UserPreferenceFilterRequest filter)
    {
        var query = _context.UserPreferences.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Language))
            query = query.Where(x => x.Language.Contains(filter.Language));
        if (!string.IsNullOrEmpty(filter.Theme))
            query = query.Where(x => x.Theme.Contains(filter.Theme));
        if (filter.ReceiveNotifications.HasValue)
            query = query.Where(x => x.ReceiveNotifications == filter.ReceiveNotifications);
        if (filter.IsActive.HasValue)
            query = query.Where(x => x.IsActive == filter.IsActive);

        var totalCount = await query.CountAsync();
        var data = await query.Skip((filter.Page - 1) * filter.PageSize)
                               .Take(filter.PageSize)
                               .ToListAsync();

        var mapped = _mapper.Map<List<UserPreferenceResponse>>(data);
        return new PaginatedResponse<UserPreferenceResponse>(mapped, totalCount, filter.Page, filter.PageSize);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.UserPreferences.FindAsync(id);
        if (entity == null) return false;

        entity.IsActive = false;
        entity.ChangedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}