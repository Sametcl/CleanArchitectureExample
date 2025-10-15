using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Dtos;
using MediatR;

namespace CleanArchitecture.Application.Features.AuthFeatures.Commands.Register
{
    public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, MessageResponse>
    {
        private readonly IAuthService authService;
        private readonly IMailService mailService;
        public RegisterCommandHandler(IAuthService authService, IMailService mailService)
        {
            this.authService = authService;
            this.mailService = mailService;
        }

        public async Task<MessageResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
           await authService.RegisterAsync(request);
           
           //await mailService.SendMailAsync(request.Email,request.FirstName);
           return new MessageResponse("User registered successfully.");
        }
    }
}
