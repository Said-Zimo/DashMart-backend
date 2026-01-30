using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Categories;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using MediatR;

namespace DashMart.Application.Products.Commands.Category
{
    public sealed record RemoveProductFromCategoryCommand
    (
        Guid CategoryId,
        Guid ProductId
        ):IRequest<Result<string>>;

    internal sealed class RemoveProductFromCategoryCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, ICategoryRepository categoryRepo, IUnitOfWork unitOfWork) 
        : IRequestHandler<RemoveProductFromCategoryCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RemoveProductFromCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateProducts))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var product = await productRepo.GetByPublicIdAsync(request.ProductId, cancellationToken);

            if (product == null) return Result<string>.Failure("Product not found", StatusCodeEnum.NotFound);

            var categoryId = await categoryRepo.GetInternalIdByPublicIdAsync(request.CategoryId, cancellationToken);

            if (categoryId <= 0) return Result<string>.Failure("Category not found", StatusCodeEnum.NotFound);

            product.DeleteFromCategory(categoryId);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Product removed from category successfully");

        }
    }

}
