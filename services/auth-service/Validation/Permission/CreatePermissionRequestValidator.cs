using AuthService.DTOs.Permission;
using FluentValidation;

namespace AuthService.Validation.Permission
{
    public class CreatePermissionRequestValidator : AbstractValidator<CreatePermissionRequest>
    {
        public CreatePermissionRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nama permission wajib diisi.")
                .MaximumLength(100).WithMessage("Nama maksimal 100 karakter.");

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("CompanyId harus lebih besar dari 0.");
        }
    }
}