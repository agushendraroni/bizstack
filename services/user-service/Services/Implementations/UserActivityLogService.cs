using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs;
using UserService.Data;
using UserService.DTOs.UserActivityLog;
using UserService.Models;
using UserService.Services.Interfaces;

namespace UserService.Services.Implementations;

public class UserActivityLogService : IUserActivityLogService
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;

    public UserActivityLogService(UserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserActivityLogResponse> CreateAsync(CreateUserActivityLogRequest request, string currentUser)
    {
        var entity = _mapper.Map<UserActivityLog>(request);
        entity.CreatedAt = DateTime.UtcNow;
        entity.CreatedBy = currentUser;
        entity.IsActive = true;

        _context.UserActivityLogs.Add(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserActivityLogResponse>(entity);
    }

    public async Task<UserActivityLogResponse?> GetByIdAsync(Guid id)
    {
        var entity = await _context.UserActivityLogs.FindAsync(id);
        return entity == null ? null : _mapper.Map<UserActivityLogResponse>(entity);
    }

    public async Task<PaginatedResponse<UserActivityLogResponse>> GetFilteredAsync(UserActivityLogFilterRequest filter)
    {
        var query = _context.UserActivityLogs.AsQueryable();

        if (filter.UserId.HasValue)
            query = query.Where(x => x.UserId == filter.UserId);
        if (!string.IsNullOrEmpty(filter.Activity))
            query = query.Where(x => x.Activity.Contains(filter.Activity));
        if (filter.CreatedAfter.HasValue)
            query = query.Where(x => x.CreatedAt >= filter.CreatedAfter);
        if (filter.CreatedBefore.HasValue)
            query = query.Where(x => x.CreatedAt <= filter.CreatedBefore);
        if (filter.IsActive.HasValue)
            query = query.Where(x => x.IsActive == filter.IsActive);

        var totalCount = await query.CountAsync();
        var data = await query.Skip((filter.Page - 1) * filter.PageSize)
                               .Take(filter.PageSize)
                               .ToListAsync();

        var mapped = _mapper.Map<List<UserActivityLogResponse>>(data);
        return new PaginatedResponse<UserActivityLogResponse>(mapped, totalCount, filter.Page, filter.PageSize);
    }
}
