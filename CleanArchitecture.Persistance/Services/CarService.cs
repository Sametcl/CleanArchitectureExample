using CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Persistance.Context;

namespace CleanArchitecture.Persistance.Services
{
    public sealed class CarService : ICarService
    {
        private readonly AppDbContext context;

        public CarService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(CreateCarCommand request, CancellationToken cancellationToken)
        {
            Car car = new ()
            {
               Name = request.Name,
               Model = request.Model,
               EnginePower =request.EnginePower,
            };
            await context.Set<Car>().AddAsync(car, cancellationToken);   
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
