using AuthService.DTOs.RolePermission;
using FluentValidation;

namespace AuthService.Validation.RolePermission
{
    public class UpdateRolePermissionRequestValidator : AbstractValidator<UpdateRolePermissionRequest>
    {
        public UpdateRolePermissionRequestValidator()
        {
            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("RoleId harus lebih besar dari 0.");
            RuleFor(x => x.PermissionId)
                .GreaterThan(0).WithMessage("PermissionId harus lebih besar dari 0.");
        }
    }
}