using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Presentation.Abstractions
{
    [ApiController] 
    [Route("api/[controller]/")]
    public abstract class ApiController : ControllerBase
    {
        public readonly IMediator _mediator;

        public ApiController(IMediator _mediator)
        {
            this._mediator = _mediator;
        }
    }
}
