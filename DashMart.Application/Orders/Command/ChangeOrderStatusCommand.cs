using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Orders;
using MediatR;

namespace DashMart.Application.Orders.Command
{
    public sealed record ChangeOrderStatusCommand
    (
        Guid OrderId,
        int NewOrderStatus
        ) : IRequest <Result<string>>;


    internal sealed class ChangeOrderStatusCommandHandler
        (ICurrentUserService currentUser, IOrderRepository orderRepo): IRequestHandler<ChangeOrderStatusCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateOrder))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            if (Enum.IsDefined(typeof(OrderStatusEnum), request.NewOrderStatus))
                return Result<string>.Failure("New order status is not valid", StatusCodeEnum.BadRequest);

            var order = await orderRepo.GetByPublicIdAsync(request.OrderId, cancellationToken);

            if (order == null)
                return Result<string>.Failure("Order not found", StatusCodeEnum.NotFound);

            order.ChangeOrderStatus(DateTime.Now, (OrderStatusEnum)request.NewOrderStatus);

            return Result<string>.Success("Order status changed successfully");


        }
    }

}
