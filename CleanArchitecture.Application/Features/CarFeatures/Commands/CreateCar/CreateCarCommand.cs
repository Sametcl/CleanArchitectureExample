using CleanArchitecture.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar
{
    public sealed class CreateCarCommand : IRequest<MessageResponse>
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public int EnginePower { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
