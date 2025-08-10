using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrganizationService.Data;
using OrganizationService.DTOs.BusinessGroup;
using OrganizationService.Models;
using OrganizationService.Services.Interfaces;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Implementations
{
    public class BusinessGroupService : IBusinessGroupService
    {
        private readonly OrganizationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBusinessGroupRequest> _createValidator;
        private readonly IValidator<UpdateBusinessGroupRequest> _updateValidator;

        public BusinessGroupService(
            OrganizationDbContext context,
            IMapper mapper,
            IValidator<CreateBusinessGroupRequest> createValidator,
            IValidator<UpdateBusinessGroupRequest> updateValidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<BusinessGroupResponse> CreateAsync(CreateBusinessGroupRequest request)
        {
            var validation = await _createValidator.ValidateAsync(request);
            if (!validation.IsValid)
                throw new ValidationException(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

            var entity = _mapper.Map<BusinessGroup>(request);
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<BusinessGroupResponse>(entity);
        }

        public async Task<BusinessGroupResponse> UpdateAsync(Guid id, UpdateBusinessGroupRequest request)
        {
            var validation = await _updateValidator.ValidateAsync(request);
            if (!validation.IsValid)
                throw new ValidationException(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

            var entity = await _context.Set<BusinessGroup>().FindAsync(id)
                ?? throw new KeyNotFoundException("BusinessGroup not found");

            _mapper.Map(request, entity);
            entity.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return _mapper.Map<BusinessGroupResponse>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Set<BusinessGroup>().FindAsync(id)
                ?? throw new KeyNotFoundException("BusinessGroup not found");

            entity.IsDeleted = true;
            entity.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BusinessGroupResponse> GetByIdAsync(Guid id)
        {
            var entity = await _context.Set<BusinessGroup>().AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
                ?? throw new KeyNotFoundException("BusinessGroup not found");

            return _mapper.Map<BusinessGroupResponse>(entity);
        }

        public async Task<PaginatedResponse<BusinessGroupResponse>> GetAllAsync(BusinessGroupFilterRequest filter)
        {
            var query = _context.Set<BusinessGroup>().AsQueryable().Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(x => x.Name.Contains(filter.Name));

            var totalCount = await query.CountAsync();
            var data = await query
                .OrderBy(x => x.Name)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result = _mapper.Map<List<BusinessGroupResponse>>(data);
            return new PaginatedResponse<BusinessGroupResponse>(result, totalCount, filter.Page, filter.PageSize);
        }
    }
}