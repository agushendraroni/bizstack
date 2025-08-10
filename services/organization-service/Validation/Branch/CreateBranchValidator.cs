using FluentValidation;
using OrganizationService.DTOs.Branch;

namespace OrganizationService.Validation.Branch
{
    public class CreateBranchRequestValidator : AbstractValidator<CreateBranchRequest>
    {
        public CreateBranchRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(200);
            RuleFor(x => x.CompanyId).NotEmpty();
        }
    }
    public class UpdateBranchRequestValidator : AbstractValidator<UpdateBranchRequest>
    {
        public UpdateBranchRequestValidator()
        {
            RuleFor(x => x.Code).MaximumLength(20);
            RuleFor(x => x.Name).MaximumLength(100);
            RuleFor(x => x.Address).MaximumLength(200);
        }
    }
}