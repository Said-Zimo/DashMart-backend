using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Orders;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Orders.Command
{
    public sealed record InsertItemQuantityCommand
    (
        Guid ProductId,
        Guid OrderId,
        int NewQuantity
        ) : IRequest<Result<string>>;

    public sealed class InsertItemQuantityCommandValidator : AbstractValidator<InsertItemQuantityCommand>
    {
        public InsertItemQuantityCommandValidator()
        {
            RuleFor(x => x.NewQuantity).GreaterThan(0).LessThanOrEqualTo(32767).WithMessage("Quantity out of valid range");

        }
    }


    internal sealed class InsertItemQuantityCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, IOrderRepository orderRepo, IUnitOfWork unitOfWork) 
        : IRequestHandler<InsertItemQuantityCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(InsertItemQuantityCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateOrder))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var order = await orderRepo.GetByPublicIdAsync(request.OrderId, cancellationToken);

            if (order == null)
                return Result<string>.Failure("Order not found", StatusCodeEnum.NotFound);

            var product = await productRepo.GetByPublicIdAsync(request.ProductId, cancellationToken);

            if (product == null)
                return Result<string>.Failure("Product not found", StatusCodeEnum.NotFound);

            order.InsertItemQuantity(product.Id,(short) request.NewQuantity);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Inserted order item quantity successfully");

        }
    }


}
