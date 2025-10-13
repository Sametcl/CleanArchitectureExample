using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.AuthFeatures.Commands.Register
{
    public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(p => p.Email).NotEmpty().WithMessage("Email bos olamaz");
            RuleFor(p => p.Email).NotNull().WithMessage("Email bos olamaz");
            RuleFor(p => p.Email).EmailAddress().WithMessage("Gecerli bir mail adresi giriniz");

            RuleFor(p => p.UserName).NotEmpty().WithMessage("Kullanici adi bos olamaz");
            RuleFor(p => p.UserName).NotNull().WithMessage("Kullanici adi bos olamaz");
            RuleFor(p => p.UserName).MinimumLength(3).WithMessage("Kullanici adi en az 3 karakter olmalidir");

            RuleFor(p => p.Password).NotEmpty().WithMessage("Sifre bos olamaz");
            RuleFor(p => p.Password).NotNull().WithMessage("Sifre bos olamaz");
            RuleFor(p => p.Password).Matches("[A-Z]").WithMessage("Sifre en az 1 adet buyuk harf icermelidir");
            RuleFor(p => p.Password).Matches("[a-z]").WithMessage("Sifre en az 1 adet kucuk harf icermelidir");
            RuleFor(p => p.Password).Matches("[0-9]").WithMessage("Sifre en az 1 adet rakam icermelidir");
            RuleFor(p => p.Password).Matches("[^a-zA-Z0-9]").WithMessage("Sifre en az 1 adet ozel karakter icermelidir");
        }
    }
}
