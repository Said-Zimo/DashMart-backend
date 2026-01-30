using DashMart.Application.Abstraction;
using DashMart.Application.Products.DTOs;
using DashMart.Application.Products.Queries.Interface;
using DashMart.Application.Results;
using MediatR;

namespace DashMart.Application.Products.Queries
{

    public sealed record GetAllProductsByCategoryIdQuery(Guid CategoryId, int PageSize, int PageNumber) 
        : IRequest<Result<IReadOnlyList<ProductListDTO>>>;


    internal sealed class GetAllProductsByCategoryIdQueryHandler
        ( IProductReadRepository productRepo) : IRequestHandler<GetAllProductsByCategoryIdQuery, Result<IReadOnlyList<ProductListDTO>>>
    {
        public async Task<Result<IReadOnlyList<ProductListDTO>>> Handle(GetAllProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            if (pageNumber <= 0)
                pageNumber = ApplicationSettings.DefaultPageNumber;

            if (pageSize <= 0)
                pageSize = ApplicationSettings.DefaultPageSize;

            return Result<IReadOnlyList<ProductListDTO>>
                .Success(await productRepo.ListByCategoryIdAsync(request.CategoryId, pageSize, pageNumber, cancellationToken));
        }

    }

}
