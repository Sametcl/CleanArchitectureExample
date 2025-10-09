using FluentValidation;

namespace CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar
{
    public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
    {
        public CreateCarCommandValidator()
        {
            RuleFor(p=>p.Name).NotEmpty().WithMessage("Arac adi bos olamaz")
                .NotNull().WithMessage("Arac adi bos olamaz")
                .MinimumLength(3).WithMessage("Arac adi 3 karakterden uzun olmali");


            RuleFor(p => p.Model).NotEmpty().WithMessage("Arac model adi bos olamaz")
                .NotNull().WithMessage("Arac modeli bos olamaz")
                .MinimumLength(3).WithMessage("Arac modeli 3 karakterden uzun olmali");


            RuleFor(p => p.EnginePower).NotEmpty().WithMessage("Arac motor gucu bos olamaz")
                .NotNull().WithMessage("Arac motor gucu bos olamaz")
                .GreaterThan(0).WithMessage("Arac gucu 0 dan buyuk olmali");   
        }
    }
}
