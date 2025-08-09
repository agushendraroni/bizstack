using FluentValidation;
using UserService.DTOs.UserProfile;
namespace UserService.Validation.UserProfile;
public class CreateUserProfileRequestValidator : AbstractValidator<CreateUserProfileRequest>
{
    public CreateUserProfileRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Address).NotEmpty();
    }
}
