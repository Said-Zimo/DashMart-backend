using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Categories;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using MediatR;

namespace DashMart.Application.Products.Commands.Category
{
    public sealed record AddProductCategoryCommand
    (
        Guid CategoryId,
        Guid ProductId
        ): IRequest<Result<string>> ;

    internal sealed class AddProductCategoryCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, ICategoryRepository categoryRepo, IUnitOfWork unitOfWork) : IRequestHandler<AddProductCategoryCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(AddProductCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateProducts))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var product = await productRepo.GetByPublicIdAsync(request.ProductId, cancellationToken);

            if (product == null) return Result<string>.Failure("Product not found", StatusCodeEnum.NotFound);

            var categoryId = await categoryRepo.GetInternalIdByPublicIdAsync(request.CategoryId, cancellationToken);

            if (categoryId <= 0) return Result<string>.Failure("Category not found", StatusCodeEnum.NotFound);

            product.AddToCategory(categoryId);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Product added to new category successfully");

        }
    }

}
