using AuthService.DTOs.Auth;
using FluentValidation;

namespace AuthService.Validation.Auth
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken wajib diisi.");
        }
    }
}