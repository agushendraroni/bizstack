using AuthService.DTOs.Role;
using FluentValidation;

namespace AuthService.Validation.Role
{
    public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
    {
        public UpdateRoleRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nama role wajib diisi.")
                .MaximumLength(100).WithMessage("Nama maksimal 100 karakter.");

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("CompanyId harus lebih besar dari 0.");
        }
    }
}