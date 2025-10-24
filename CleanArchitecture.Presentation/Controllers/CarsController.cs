using CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar;
using CleanArchitecture.Application.Features.CarFeatures.Queries.GetAllCar;
using CleanArchitecture.Domain.Dtos;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Authorization;
using CleanArchitecture.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pandorax.PagedList;

namespace CleanArchitecture.Presentation.Controllers
{
    public sealed class CarsController : ApiController
    {
        public CarsController(IMediator _mediator) : base(_mediator){}


        [RoleFilter("Create")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm]CreateCarCommand request, CancellationToken cancellationToken)
        {
            MessageResponse response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }


        [RoleFilter("GetAll")]
        [HttpPost("[action]")]
        public async Task<IActionResult> GetAll(GetAllCarQuery request, CancellationToken cancellationToken)
        {
            IPagedList<Car> response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
