using FluentValidation;
using OrganizationService.DTOs.JobTitle;

namespace OrganizationService.Validation.JobTitle
{
    public class CreateJobTitleRequestValidator : AbstractValidator<CreateJobTitleRequest>
    {
        public CreateJobTitleRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CompanyId).NotEmpty();
        }
    }

    public class UpdateJobTitleRequestValidator : AbstractValidator<UpdateJobTitleRequest>
    {
        public UpdateJobTitleRequestValidator()
        {
            RuleFor(x => x.Title).MaximumLength(100);
        }
    }
}