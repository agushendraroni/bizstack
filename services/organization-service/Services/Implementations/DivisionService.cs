using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrganizationService.Data;
using OrganizationService.DTOs.Division;
using OrganizationService.Models;
using OrganizationService.Services.Interfaces;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Implementations
{
    public class DivisionService : IDivisionService
    {
        private readonly OrganizationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDivisionRequest> _createValidator;
        private readonly IValidator<UpdateDivisionRequest> _updateValidator;

        public DivisionService(
            OrganizationDbContext context,
            IMapper mapper,
            IValidator<CreateDivisionRequest> createValidator,
            IValidator<UpdateDivisionRequest> updateValidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<DivisionResponse> CreateAsync(CreateDivisionRequest request)
        {
            var validation = await _createValidator.ValidateAsync(request);
            if (!validation.IsValid)
                throw new ValidationException(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

            var entity = _mapper.Map<Division>(request);
            _context.Divisions.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<DivisionResponse>(entity);
        }

        public async Task<DivisionResponse> UpdateAsync(Guid id, UpdateDivisionRequest request)
        {
            var validation = await _updateValidator.ValidateAsync(request);
            if (!validation.IsValid)
                throw new ValidationException(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

            var entity = await _context.Divisions.FindAsync(id)
                ?? throw new KeyNotFoundException("Division not found");

            _mapper.Map(request, entity);
            entity.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return _mapper.Map<DivisionResponse>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Divisions.FindAsync(id)
                ?? throw new KeyNotFoundException("Division not found");

            entity.IsDeleted = true;
            entity.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DivisionResponse> GetByIdAsync(Guid id)
        {
            var entity = await _context.Divisions.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
                ?? throw new KeyNotFoundException("Division not found");

            return _mapper.Map<DivisionResponse>(entity);
        }

        public async Task<PaginatedResponse<DivisionResponse>> GetAllAsync(DivisionFilterRequest filter)
        {
            var query = _context.Divisions.AsQueryable().Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(x => x.Name.Contains(filter.Name));
            if (filter.CompanyId.HasValue)
                query = query.Where(x => x.CompanyId == filter.CompanyId);

            var totalCount = await query.CountAsync();
            var data = await query
                .OrderBy(x => x.Name)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result = _mapper.Map<List<DivisionResponse>>(data);
            return new PaginatedResponse<DivisionResponse>(result, totalCount, filter.Page, filter.PageSize);
        }
    }
}