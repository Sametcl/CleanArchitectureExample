using FluentValidation;

namespace CleanArchitecture.Application.Features.UserRoleFeatures.Commands.CreateUserRole
{
    public sealed class CreateUserRoleCommandValidator : AbstractValidator<CreateUserRoleCommand>
    {
        public CreateUserRoleCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId bos olamaz");
            RuleFor(x => x.UserId).NotNull().WithMessage("UserId bos olamaz");

            RuleFor(x => x.RoleId).NotEmpty().WithMessage("RoleId bos olamaz");
            RuleFor(x => x.RoleId).NotNull().WithMessage("RoleId bos olamaz");
        }
    }
}
