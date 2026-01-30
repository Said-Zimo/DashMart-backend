using DashMart.Application.Products.DTOs;
using DashMart.Application.Products.Queries.Interface;
using DashMart.Application.Results;
using MediatR;

namespace DashMart.Application.Products.Queries
{

    public sealed record GetProductByIdQuery(Guid ProductId) : IRequest<Result< ProductViewDto>>;

    internal sealed class GetProductByIdQueryHandler
        (IProductReadRepository productRepo) : IRequestHandler<GetProductByIdQuery, Result<ProductViewDto>>
    {
        public async Task<Result<ProductViewDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await productRepo.GetByIdAsync(request.ProductId, cancellationToken);

            if (product == null)
            {
                return Result<ProductViewDto>.Failure("Product not found", StatusCodeEnum.NotFound);
            }

            return Result<ProductViewDto>.Success(product);
        }
    }
}
