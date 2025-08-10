using FluentValidation;
using OrganizationService.DTOs.CostCenter;

namespace OrganizationService.Validation.CostCenter
{
    public class CreateCostCenterRequestValidator : AbstractValidator<CreateCostCenterRequest>
    {
        public CreateCostCenterRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.DivisionId).NotEmpty();
        }
    }

    public class UpdateCostCenterRequestValidator : AbstractValidator<UpdateCostCenterRequest>
    {
        public UpdateCostCenterRequestValidator()
        {
            RuleFor(x => x.Code).MaximumLength(20);
            RuleFor(x => x.Name).MaximumLength(100);
        }
    }
}