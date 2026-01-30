using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Orders;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using MediatR;

namespace DashMart.Application.Orders.Command
{
    public sealed record DeleteOrderItemCommand
    (
        Guid OrderId,
        Guid ProductId
        ) : IRequest<Result<string>>;


    internal sealed class DeleteOrderItemCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, IOrderRepository orderRepo, IUnitOfWork unitOfWork) 
        : IRequestHandler<DeleteOrderItemCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateOrder))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var order = await orderRepo.GetByPublicIdAsync(request.OrderId, cancellationToken);

            if (order == null)
                return Result<string>.Failure("Order not found", StatusCodeEnum.NotFound);

            var product = await productRepo.GetByPublicIdAsync(request.ProductId, cancellationToken);

            if (product == null)
                return Result<string>.Failure("Product not found", StatusCodeEnum.NotFound);

            order.DeleteItem(product.Id);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Deleted order item successfully");
        }
    }


}
