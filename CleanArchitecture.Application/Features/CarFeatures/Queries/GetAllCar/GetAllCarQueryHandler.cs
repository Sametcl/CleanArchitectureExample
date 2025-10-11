using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;
using GenericRepository;
using MediatR;

namespace CleanArchitecture.Application.Features.CarFeatures.Queries.GetAllCar
{
    public sealed class GetAllCarQueryHandler : IRequestHandler<GetAllCarQuery, IList<Car>>
    {
        private readonly ICarService _carService;
        private readonly IUnitOfWork _unitOfWork;
        public GetAllCarQueryHandler(ICarService carService, IUnitOfWork unitOfWork)
        {
            _carService = carService;
            _unitOfWork = unitOfWork;
        }
        public async Task<IList<Car>> Handle(GetAllCarQuery request, CancellationToken cancellationToken)
        {
            IList<Car> cars = await _carService.GetAllAsync(request, cancellationToken);
            return cars;
        }
    }
}
