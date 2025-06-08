using AuthService.DTOs.UserPermission;
using FluentValidation;

namespace AuthService.Validation.UserPermission
{
    public class CreateUserPermissionRequestValidator : AbstractValidator<CreateUserPermissionRequest>
    {
        public CreateUserPermissionRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId harus lebih besar dari 0.");
            RuleFor(x => x.PermissionId)
                .GreaterThan(0).WithMessage("PermissionId harus lebih besar dari 0.");
        }
    }
}