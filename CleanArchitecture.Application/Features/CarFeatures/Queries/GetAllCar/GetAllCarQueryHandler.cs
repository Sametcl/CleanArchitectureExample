using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Pandorax.PagedList;

namespace CleanArchitecture.Application.Features.CarFeatures.Queries.GetAllCar
{
    public sealed class GetAllCarQueryHandler : IRequestHandler<GetAllCarQuery, IPagedList<Car>>
    {
        private readonly ICarService _carService;
        public GetAllCarQueryHandler(ICarService carService)
        {
            _carService = carService;
        }
        public async Task<IPagedList<Car>> Handle(GetAllCarQuery request, CancellationToken cancellationToken)
        {
            IPagedList<Car> cars = await _carService.GetAllAsync(request, cancellationToken);
            return cars;
        }
    }
}
