using AutoMapper;
using CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Persistance.Context;

namespace CleanArchitecture.Persistance.Services
{
    public sealed class CarService : ICarService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public CarService(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task CreateAsync(CreateCarCommand request, CancellationToken cancellationToken)
        {
            Car car = mapper.Map<Car>(request);
            await context.Set<Car>().AddAsync(car, cancellationToken);   
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
