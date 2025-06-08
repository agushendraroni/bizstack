using AuthService.DTOs.Menu;
using FluentValidation;

namespace AuthService.Validation.Menu
{
    public class UpdateMenuRequestValidator : AbstractValidator<UpdateMenuRequest>
    {
        public UpdateMenuRequestValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("CompanyId harus lebih besar dari 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nama menu wajib diisi.")
                .MaximumLength(100).WithMessage("Nama maksimal 100 karakter.");

            RuleFor(x => x.Url)
                .MaximumLength(255).WithMessage("Url maksimal 255 karakter.");

            RuleFor(x => x.Icon)
                .MaximumLength(100).WithMessage("Icon maksimal 100 karakter.");

            RuleFor(x => x.OrderIndex)
                .GreaterThanOrEqualTo(0).WithMessage("OrderIndex tidak boleh negatif.");
        }
    }
}