using CleanArchitecture.Domain.Entities;
using MediatR;
using Pandorax.PagedList;

namespace CleanArchitecture.Application.Features.CarFeatures.Queries.GetAllCar
{
    public sealed record class GetAllCarQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string Search = "") : IRequest<IPagedList<Car>>;
}
