using FluentValidation;

namespace CleanArchitecture.Application.Features.RoleFeatures.Commands.CreateRole
{
    public sealed class CreateRoleCommandValidator :AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(r => r.Name).NotEmpty().WithMessage("Rol adi bos gecilemez");
            RuleFor(r => r.Name).NotNull().WithMessage("Rol adi bos gecilemez");
        }
    }
}
