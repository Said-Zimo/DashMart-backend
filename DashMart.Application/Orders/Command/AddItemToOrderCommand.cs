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
    public sealed record AddItemToOrderCommand
    (
        Guid OrderId,
        Guid ProductId,
        int Quantity
        ) : IRequest<Result<string>>;


    public sealed class AddItemToOrderCommandValidator : AbstractValidator<AddItemToOrderCommand>
    {

        public AddItemToOrderCommandValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0).LessThanOrEqualTo(32767).WithMessage("Quantity out of valid range");            
        }
    }


    internal sealed class AddItemToOrderCommandHandler
        (ICurrentUserService currentUser, IOrderRepository orderRepo, IProductRepository productRepo, IUnitOfWork unitOfWork)
        : IRequestHandler<AddItemToOrderCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(AddItemToOrderCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateOrder))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var order = await orderRepo.GetByPublicIdAsync(request.OrderId, cancellationToken);

            if (order == null)
                return Result<string>.Failure("Order not found", StatusCodeEnum.NotFound);

            var product = await productRepo.GetByPublicIdAsync(request.ProductId, cancellationToken);

            if (product == null)
                return Result<string>.Failure("Product not found", StatusCodeEnum.NotFound);

            if (!product.IsActive)
                return Result<string>.Failure("Product not active", StatusCodeEnum.NotFound);


            order.AddItem(product.Id ,(short)request.Quantity , product.Price);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Item added to order successfully");


        }
    }

}
