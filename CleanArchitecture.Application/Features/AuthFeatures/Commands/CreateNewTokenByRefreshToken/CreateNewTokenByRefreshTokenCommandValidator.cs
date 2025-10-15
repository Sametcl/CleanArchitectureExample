using FluentValidation;

namespace CleanArchitecture.Application.Features.AuthFeatures.Commands.CreateNewTokenByRefreshToken
{
    public sealed class CreateNewTokenByRefreshTokenCommandValidator : AbstractValidator<CreateNewTokenByRefreshTokenCommand>
    {
        public CreateNewTokenByRefreshTokenCommandValidator()
        {
            RuleFor(p => p.UserId).NotEmpty().WithMessage("User bilgisi bos olamaz.");
            RuleFor(p => p.UserId).NotNull().WithMessage("User bilgisi bos olamaz.");


            RuleFor(p => p.RefreshToken).NotEmpty().WithMessage("Refresh Token bilgisi bos olamaz.");
            RuleFor(p => p.UserId).NotNull().WithMessage("Refresh Token bilgisi bos olamaz.");
        }
    }
}
