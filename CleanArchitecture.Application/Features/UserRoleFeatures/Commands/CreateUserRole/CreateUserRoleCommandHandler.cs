using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Dtos;
using MediatR;

namespace CleanArchitecture.Application.Features.UserRoleFeatures.Commands.CreateUserRole
{
    public sealed class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, MessageResponse>
    {
        private readonly IUserRoleService userRoleService;

        public CreateUserRoleCommandHandler(IUserRoleService userRoleService)
        {
            this.userRoleService = userRoleService;
        }

        public async Task<MessageResponse> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
        {
            await userRoleService.CreateAsync(request, cancellationToken);
            return new MessageResponse("Kullaniciya rol basariyla atandi.");
        }
    }
}
