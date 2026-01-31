using DashMart.Application.CurrentUserService;
using DashMart.Application.Customers.Query.Interface;
using DashMart.Application.Orders.DTOs;
using DashMart.Application.Orders.Query.Interface;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Customers;
using DashMart.Domain.People.Users;
using MediatR;

namespace DashMart.Application.Orders.Query
{
    public sealed record GetOrderByIdQuery
    (
        Guid OrderId
        ) : IRequest<Result<OrderViewDto>>;


    internal sealed class GetOrderByIdQueryHandler
        (ICurrentUserService currentUser,ICustomerRepository customerReadRepo, IOrderReadRepository orderReadRepo): IRequestHandler<GetOrderByIdQuery, Result<OrderViewDto>>
    {
        public async Task<Result<OrderViewDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await orderReadRepo.GetByIdAsync(request.OrderId, cancellationToken);

            if (order == null)
                return Result<OrderViewDto>.Failure("Order not found", StatusCodeEnum.NotFound);

            var customer = await customerReadRepo.GetByIdAsync(order.CustomerId);

            if (customer == null)
                return Result<OrderViewDto>.Failure("Order owner not found", StatusCodeEnum.NotFound);

            var isOwner = customer.PublicId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.ShowOrders);

            if (!isOwner && !isUser)
                return Result<OrderViewDto>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            return Result<OrderViewDto>.Success(order);
        }
    }

}
