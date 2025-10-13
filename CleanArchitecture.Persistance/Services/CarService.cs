using AutoMapper;
using CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar;
using CleanArchitecture.Application.Features.CarFeatures.Queries.GetAllCar;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Persistance.Context;
using GenericRepository;
using Pandorax.PagedList;
using Pandorax.PagedList.EntityFrameworkCore;

namespace CleanArchitecture.Persistance.Services
{
    public sealed class CarService : ICarService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICarRepository carRepository;
        public CarService(AppDbContext context, IMapper mapper, ICarRepository carRepository, IUnitOfWork unitOfWork)
        {
            this.context = context;
            this.mapper = mapper;
            this.carRepository = carRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(CreateCarCommand request, CancellationToken cancellationToken)
        {
            Car car = mapper.Map<Car>(request);

            await carRepository.AddAsync(car, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //await context.Set<Car>().AddAsync(car, cancellationToken);   
            //await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IPagedList<Car>> GetAllAsync(GetAllCarQuery request, CancellationToken cancellationToken)
        {
            IPagedList<Car> cars = await carRepository
                .Where(p => p.Name.ToLower().Contains(request.Search.ToLower()))
                .ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);
            
            return cars;
        }
    }
}
