using FluentValidation;
using OrganizationService.DTOs.Division;

namespace OrganizationService.Validation.Division
{
    public class CreateDivisionRequestValidator : AbstractValidator<CreateDivisionRequest>
    {
        public CreateDivisionRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CompanyId).NotEmpty();
        }
    }

    public class UpdateDivisionRequestValidator : AbstractValidator<UpdateDivisionRequest>
    {
        public UpdateDivisionRequestValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100);
        }
    }
}