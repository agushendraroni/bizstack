using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs;
using UserService.Data;
using UserService.DTOs.UserProfile;
using UserService.Models;

namespace UserService.Services.Interfaces;

public class UserProfileService : IUserProfileService
{
    private readonly IMapper _mapper;
    private readonly UserDbContext _dbContext;

    public UserProfileService(IMapper mapper, UserDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<UserProfileResponse> CreateAsync(CreateUserProfileRequest request)
    {
        var entity = _mapper.Map<UserProfile>(request);
        _dbContext.UserProfiles.Add(entity);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<UserProfileResponse>(entity);
    }

    public async Task<UserProfileResponse> UpdateAsync(Guid id, UpdateUserProfileRequest request)
    {
        var entity = await _dbContext.UserProfiles.FindAsync(id);
        if (entity == null) throw new Exception("UserProfile not found");

        _mapper.Map(request, entity);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<UserProfileResponse>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _dbContext.UserProfiles.FindAsync(id);
        if (entity == null) return false;

        _dbContext.UserProfiles.Remove(entity);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<UserProfileResponse> GetByIdAsync(Guid id)
    {
        var entity = await _dbContext.UserProfiles.FindAsync(id);
        return _mapper.Map<UserProfileResponse>(entity);
    }

    public async Task<PaginatedResponse<UserProfileResponse>> GetFilteredAsync(UserProfileFilterRequest filter)
    {
        var query = _dbContext.UserProfiles.AsQueryable();

        if (!string.IsNullOrEmpty(filter.FullName))
            query = query.Where(x => x.FullName.Contains(filter.FullName));

        if (!string.IsNullOrEmpty(filter.PhoneNumber))
            query = query.Where(x => x.PhoneNumber != null && x.PhoneNumber.Contains(filter.PhoneNumber));

        if (filter.CreatedAfter.HasValue)
            query = query.Where(x => x.CreatedAt >= filter.CreatedAfter);

        if (filter.CreatedBefore.HasValue)
            query = query.Where(x => x.CreatedAt <= filter.CreatedBefore);

        if (filter.IsActive.HasValue)
            query = query.Where(x => x.IsActive == filter.IsActive);

        var total = await query.CountAsync();

        var data = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PaginatedResponse<UserProfileResponse>(
            _mapper.Map<List<UserProfileResponse>>(data), total, filter.Page, filter.PageSize);
    }
}
