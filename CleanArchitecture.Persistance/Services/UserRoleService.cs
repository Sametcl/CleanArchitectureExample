using CleanArchitecture.Application.Features.UserRoleFeatures.Commands.CreateUserRole;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;
using GenericRepository;

namespace CleanArchitecture.Persistance.Services
{
    public sealed class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserRoleService(IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork)
        {
            this.userRoleRepository = userRoleRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(CreateUserRoleCommand request, CancellationToken cancellationToken)
        {
            UserRole userRole = new()
            {
                UserId = request.UserId,
                RoleId = request.RoleId
            };
            await userRoleRepository.AddAsync(userRole, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
