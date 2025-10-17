using CleanArchitecture.Application.Features.UserRoleFeatures.Commands.CreateUserRole;
using CleanArchitecture.Domain.Dtos;
using CleanArchitecture.Infrastructure.Authorization;
using CleanArchitecture.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Presentation.Controllers
{
    public sealed class UserRolesController : ApiController
    {
        public UserRolesController(IMediator _mediator) : base(_mediator)
        {
        }

        [HttpPost("[action]")]
        [RoleFilter("Create")]
        public async Task<IActionResult> Create(CreateUserRoleCommand request, CancellationToken cancellationToken)
        {
            MessageResponse response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
