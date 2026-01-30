using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Products.Commands
{
    public sealed record ReduceStockQuantityCommand
    (
        Guid ProductId,
        int StockQuantity
        ) : IRequest <Result<string>>;


    public sealed class ReduceStockQuantityCommandValidator : AbstractValidator<ReduceStockQuantityCommand>
    {
        public ReduceStockQuantityCommandValidator()
        {
            RuleFor(x => x.StockQuantity).GreaterThan(0).WithMessage("Inserted stock must be greater than 0");
        }
    }

    internal sealed class ReduceStockQuantityCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, IUnitOfWork unitOfWork)
        : IRequestHandler<ReduceStockQuantityCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ReduceStockQuantityCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateProducts))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var product = await productRepo.GetByPublicIdAsync(request.ProductId , cancellationToken);

            if (product == null) return Result<string>.Failure("Product cannot found", StatusCodeEnum.NotFound); 

            product.ReduceStockQuantity(request.StockQuantity);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Product stock updated successfully");

        }
    }


}
