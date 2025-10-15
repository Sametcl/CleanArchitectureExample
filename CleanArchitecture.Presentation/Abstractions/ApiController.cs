using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Presentation.Abstractions
{
    [ApiController] 
    [Route("api/[controller]/")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public abstract class ApiController : ControllerBase
    {
        public readonly IMediator _mediator;

        public ApiController(IMediator _mediator)
        {
            this._mediator = _mediator;
        }
    }
}
