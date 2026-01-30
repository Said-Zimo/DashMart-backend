using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Products.Commands
{
    public sealed record InsertProductStockQuantityCommand
    (
        Guid ProductId,
        int StockQuantity
        ) : IRequest <Result<string>>;


    public sealed class InsertProductStockQuantityCommandValidator : AbstractValidator<InsertProductStockQuantityCommand>
    {
        public InsertProductStockQuantityCommandValidator()
        {
            RuleFor(x => x.StockQuantity).GreaterThan(0).WithMessage("Inserted stock must be greater than 0");
        }
    }

    internal sealed class InsertProductStockQuantityCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo , IUnitOfWork unitOfWork): IRequestHandler<InsertProductStockQuantityCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(InsertProductStockQuantityCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateProducts))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var product = await productRepo.GetByPublicIdAsync(request.ProductId , cancellationToken);

            if (product == null) return Result<string>.Failure("Product not found", StatusCodeEnum.NotFound);

            product.InsertStockQuantity(request.StockQuantity);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Product stock updated successfully");
        }
    }


}
