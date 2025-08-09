using FluentValidation;
using UserService.DTOs.UserProfile;
namespace UserService.Validation.UserProfile;
public class UpdateUserProfileRequestValidator : AbstractValidator<UpdateUserProfileRequest>
{
    public UpdateUserProfileRequestValidator()
    {
        RuleFor(x => x.FullName).MaximumLength(100);
        RuleFor(x => x.PhoneNumber).MaximumLength(20);
    }
}