using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Couriers;
using DashMart.Domain.People.Users;
using DashMart.Domain.Orders;
using DashMart.Domain.UnitOfWorks;
using MediatR;

namespace DashMart.Application.Couriers.Command
{
    public sealed record SetCourierToOrderCommand
    (
        Guid CourierId,
        Guid OrderId
        ) : IRequest<Result<string>>;


    internal sealed class SetCourierToOrderCommandHandler
        (ICurrentUserService currentUser, ICourierRepository courierRepo, IOrderRepository orderRepo, IUnitOfWork unitOfWork) 
        : IRequestHandler<SetCourierToOrderCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(SetCourierToOrderCommand request, CancellationToken cancellationToken)
        {
            var courier = await courierRepo.GetByPublicIdAsync(request.CourierId, cancellationToken);

            if (courier == null) return Result<string>.Failure("Courier not found", StatusCodeEnum.NotFound);

            var isOwner = courier.PublicId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.UpdateOrder);

            if (!isOwner && !isUser)
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            if (!courier.IsReadyToWork) return Result<string>.Failure("Courier not ready to work yet", StatusCodeEnum.BadRequest);

            var order = await orderRepo.GetByPublicIdAsync(request.OrderId, cancellationToken);

            if (order == null) return Result<string>.Failure("Order not found", StatusCodeEnum.NotFound);

            order.SetCourier(courier.Id);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("The courier was assigned to the order");

        }
    }


}
