using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using MediatR;

namespace DashMart.Application.Products.Commands
{
    public sealed record DeleteProductCommand
    (
        Guid ProductId
        ) : IRequest <Result<string>>;


    internal sealed class DeleteProductCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.DeleteProducts))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var product = await productRepo.GetByPublicIdAsync(request.ProductId, cancellationToken);

            if (product == null) return Result<string>.Failure("Product not found", StatusCodeEnum.NotFound);

            productRepo.Delete(product);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Product deleted successfully");
            
        }
    }

}
