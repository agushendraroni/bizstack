using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrganizationService.Data;
using OrganizationService.DTOs.CostCenter;
using OrganizationService.Models;
using OrganizationService.Services.Interfaces;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Implementations
{
    public class CostCenterService : ICostCenterService
    {
        private readonly OrganizationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCostCenterRequest> _createValidator;
        private readonly IValidator<UpdateCostCenterRequest> _updateValidator;

        public CostCenterService(
            OrganizationDbContext context,
            IMapper mapper,
            IValidator<CreateCostCenterRequest> createValidator,
            IValidator<UpdateCostCenterRequest> updateValidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<CostCenterResponse> CreateAsync(CreateCostCenterRequest request)
        {
            var validation = await _createValidator.ValidateAsync(request);
            if (!validation.IsValid)
                throw new ValidationException(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

            var entity = _mapper.Map<CostCenter>(request);
            _context.CostCenters.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<CostCenterResponse>(entity);
        }

        public async Task<CostCenterResponse> UpdateAsync(Guid id, UpdateCostCenterRequest request)
        {
            var validation = await _updateValidator.ValidateAsync(request);
            if (!validation.IsValid)
                throw new ValidationException(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

            var entity = await _context.CostCenters.FindAsync(id)
                ?? throw new KeyNotFoundException("CostCenter not found");

            _mapper.Map(request, entity);
            entity.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return _mapper.Map<CostCenterResponse>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.CostCenters.FindAsync(id)
                ?? throw new KeyNotFoundException("CostCenter not found");

            entity.IsDeleted = true;
            entity.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CostCenterResponse> GetByIdAsync(Guid id)
        {
            var entity = await _context.CostCenters.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
                ?? throw new KeyNotFoundException("CostCenter not found");

            return _mapper.Map<CostCenterResponse>(entity);
        }

        public async Task<PaginatedResponse<CostCenterResponse>> GetAllAsync(CostCenterFilterRequest filter)
        {
            var query = _context.CostCenters.AsQueryable().Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(x => x.Name.Contains(filter.Name));
            if (filter.DivisionId.HasValue)
                query = query.Where(x => x.DivisionId == filter.DivisionId);

            var totalCount = await query.CountAsync();
            var data = await query
                .OrderBy(x => x.Name)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result = _mapper.Map<List<CostCenterResponse>>(data);
            return new PaginatedResponse<CostCenterResponse>(result, totalCount, filter.Page, filter.PageSize);
        }
    }
}