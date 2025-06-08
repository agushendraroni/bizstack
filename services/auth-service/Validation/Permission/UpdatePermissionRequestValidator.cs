using AuthService.DTOs.Permission;
using FluentValidation;

namespace AuthService.Validation.Permission
{
    public class UpdatePermissionRequestValidator : AbstractValidator<UpdatePermissionRequest>
    {
        public UpdatePermissionRequestValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("CompanyId harus lebih besar dari 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nama permission wajib diisi.")
                .MaximumLength(100).WithMessage("Nama maksimal 100 karakter.");

            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("Deskripsi maksimal 255 karakter.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}