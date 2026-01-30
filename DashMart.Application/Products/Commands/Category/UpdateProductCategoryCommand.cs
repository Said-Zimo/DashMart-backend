using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Categories;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using MediatR;

namespace DashMart.Application.Products.Commands.Category
{
    public sealed record UpdateProductCategoryCommand
    (
       Guid ProductId,
       Guid CurrentCategoryId,
       Guid NewCategoryId
        ) : IRequest<Result<string>>;


    internal sealed class UpdateProductCategoryCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, ICategoryRepository categoryRepo, IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductCategoryCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateProducts))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var product = await productRepo.GetByPublicIdAsync(request.ProductId, cancellationToken);

            if (product == null) return Result<string>.Failure("Product not found", StatusCodeEnum.NotFound);

            var CurrentCategoryId = await categoryRepo.GetInternalIdByPublicIdAsync(request.CurrentCategoryId, cancellationToken);
            var NewCategoryId = await categoryRepo.GetInternalIdByPublicIdAsync(request.NewCategoryId, cancellationToken);

            if (CurrentCategoryId <= 0) return Result<string>.Failure("Current Category not found", StatusCodeEnum.NotFound);
            if (NewCategoryId <= 0) return Result<string>.Failure("New Category not found", StatusCodeEnum.NotFound);

            product.UpdateCategory(CurrentCategoryId, NewCategoryId);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Product category updated successfully");

        }
    }

}
