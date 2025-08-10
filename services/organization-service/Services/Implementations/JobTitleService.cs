using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrganizationService.Data;
using OrganizationService.DTOs.JobTitle;
using OrganizationService.Models;
using OrganizationService.Services.Interfaces;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Implementations
{
    public class JobTitleService : IJobTitleService
    {
        private readonly OrganizationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateJobTitleRequest> _createValidator;
        private readonly IValidator<UpdateJobTitleRequest> _updateValidator;

        public JobTitleService(
            OrganizationDbContext context,
            IMapper mapper,
            IValidator<CreateJobTitleRequest> createValidator,
            IValidator<UpdateJobTitleRequest> updateValidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<JobTitleResponse> CreateAsync(CreateJobTitleRequest request)
        {
            var validation = await _createValidator.ValidateAsync(request);
            if (!validation.IsValid)
                throw new ValidationException(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

            var entity = _mapper.Map<JobTitle>(request);
            _context.JobTitles.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<JobTitleResponse>(entity);
        }

        public async Task<JobTitleResponse> UpdateAsync(Guid id, UpdateJobTitleRequest request)
        {
            var validation = await _updateValidator.ValidateAsync(request);
            if (!validation.IsValid)
                throw new ValidationException(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

            var entity = await _context.JobTitles.FindAsync(id)
                ?? throw new KeyNotFoundException("JobTitle not found");

            _mapper.Map(request, entity);
            entity.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return _mapper.Map<JobTitleResponse>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.JobTitles.FindAsync(id)
                ?? throw new KeyNotFoundException("JobTitle not found");

            entity.IsDeleted = true;
            entity.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<JobTitleResponse> GetByIdAsync(Guid id)
        {
            var entity = await _context.JobTitles.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
                ?? throw new KeyNotFoundException("JobTitle not found");

            return _mapper.Map<JobTitleResponse>(entity);
        }

        public async Task<PaginatedResponse<JobTitleResponse>> GetAllAsync(JobTitleFilterRequest filter)
        {
            var query = _context.JobTitles.AsQueryable().Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(x => x.Title.Contains(filter.Title));
            if (filter.CompanyId.HasValue)
                query = query.Where(x => x.CompanyId == filter.CompanyId);

            var totalCount = await query.CountAsync();
            var data = await query
                .OrderBy(x => x.Title)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result = _mapper.Map<List<JobTitleResponse>>(data);
            return new PaginatedResponse<JobTitleResponse>(result, totalCount, filter.Page, filter.PageSize);
        }
    }
}