using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;
using GenericRepository;
using MediatR;
using Pandorax.PagedList;

namespace CleanArchitecture.Application.Features.CarFeatures.Queries.GetAllCar
{
    public sealed class GetAllCarQueryHandler : IRequestHandler<GetAllCarQuery, IPagedList<Car>>
    {
        private readonly ICarService _carService;
        private readonly IUnitOfWork _unitOfWork;
        public GetAllCarQueryHandler(ICarService carService, IUnitOfWork unitOfWork)
        {
            _carService = carService;
            _unitOfWork = unitOfWork;
        }
        public async Task<IPagedList<Car>> Handle(GetAllCarQuery request, CancellationToken cancellationToken)
        {
            IPagedList<Car> cars = await _carService.GetAllAsync(request, cancellationToken);
            return cars;
        }
    }
}
