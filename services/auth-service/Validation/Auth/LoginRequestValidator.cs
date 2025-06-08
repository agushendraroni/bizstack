using AuthService.DTOs.Auth;
using FluentValidation;

namespace AuthService.Validation.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username wajib diisi.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password wajib diisi.");
        }
    }
}