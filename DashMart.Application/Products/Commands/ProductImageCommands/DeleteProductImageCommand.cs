using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using MediatR;

namespace DashMart.Application.Products.Commands.ProductImageCommands
{
    public sealed record DeleteProductImageCommand
    (
        Guid ProductPublicId,
        Guid ImagePublicId

        ) : IRequest<Result<string>>;


    internal sealed class DeleteProductImageCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductImageCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateProducts))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var product = await productRepo.GetByPublicIdAsync(request.ProductPublicId , cancellationToken);

            if (product == null) return Result<string>.Failure($"Product with public Id {request.ProductPublicId} not found", StatusCodeEnum.NotFound);

            product.RemoveImageFromGallery(request.ImagePublicId);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Product image deleted successfully");

        }
    }


}
