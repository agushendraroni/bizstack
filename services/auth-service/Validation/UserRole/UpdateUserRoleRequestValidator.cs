using AuthService.DTOs.UserRole;
using FluentValidation;

namespace AuthService.Validation.UserRole
{
    public class UpdateUserRoleRequestValidator : AbstractValidator<UpdateUserRoleRequest>
    {
        public UpdateUserRoleRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId harus lebih besar dari 0.");
            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("RoleId harus lebih besar dari 0.");
        }
    }
}