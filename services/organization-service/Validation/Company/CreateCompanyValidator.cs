using FluentValidation;
using OrganizationService.DTOs.Company;
namespace OrganizationService.Validation.Company;
public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
{
    public CreateCompanyRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}
