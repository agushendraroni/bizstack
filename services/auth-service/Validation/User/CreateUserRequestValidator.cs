using AuthService.DTOs.User;
using FluentValidation;

namespace AuthService.Validation.User
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username wajib diisi.")
                .Length(3, 100).WithMessage("Username harus antara 3 sampai 100 karakter.")
                .Matches(@"^[a-zA-Z0-9_.-]+$").WithMessage("Username hanya boleh huruf, angka, titik, underscore, dan strip.");

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("CompanyId harus lebih besar dari 0.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password wajib diisi.")
                .MinimumLength(6).WithMessage("Password minimal 6 karakter.");
        }
    }
}