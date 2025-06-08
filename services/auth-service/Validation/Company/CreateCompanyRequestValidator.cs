using AuthService.DTOs.Company;
using FluentValidation;

namespace AuthService.Validation.Company
{
    public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
    {
        public CreateCompanyRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nama perusahaan wajib diisi.")
                .MaximumLength(100).WithMessage("Nama maksimal 100 karakter.");
        }
    }
}