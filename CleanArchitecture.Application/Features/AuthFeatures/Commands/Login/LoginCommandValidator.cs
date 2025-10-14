using FluentValidation;

namespace CleanArchitecture.Application.Features.AuthFeatures.Commands.Login
{
    public sealed class LoginCommandValidator: AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(p => p.UserNameOrEmail).NotEmpty().WithMessage("Kullanici adi ya da email bos gecilemez");
            RuleFor(p => p.UserNameOrEmail).NotNull().WithMessage("Kullanici adi ya da email bos gecilemez");


            RuleFor(p => p.Password).NotEmpty().WithMessage("Sifre bos gecilemez");
            RuleFor(p => p.Password).NotNull().WithMessage("Sifre bos gecilemez");
        }
    }
}
