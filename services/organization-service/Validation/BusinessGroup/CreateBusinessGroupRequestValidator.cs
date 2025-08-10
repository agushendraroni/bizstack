using FluentValidation;
using OrganizationService.DTOs.BusinessGroup;

namespace OrganizationService.Validation.BusinessGroup
{
    public class CreateBusinessGroupRequestValidator : AbstractValidator<CreateBusinessGroupRequest>
    {
        public CreateBusinessGroupRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }

    public class UpdateBusinessGroupRequestValidator : AbstractValidator<UpdateBusinessGroupRequest>
    {
        public UpdateBusinessGroupRequestValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100);
        }
    }
}