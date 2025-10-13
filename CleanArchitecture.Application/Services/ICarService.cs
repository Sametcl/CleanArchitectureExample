using CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar;
using CleanArchitecture.Application.Features.CarFeatures.Queries.GetAllCar;
using CleanArchitecture.Domain.Entities;
using Pandorax.PagedList;

namespace CleanArchitecture.Application.Services
{
    public interface ICarService
    {
        Task CreateAsync(CreateCarCommand request, CancellationToken cancellationToken);
        Task<IPagedList<Car>> GetAllAsync(GetAllCarQuery request ,CancellationToken cancellationToken);  
    }
}
