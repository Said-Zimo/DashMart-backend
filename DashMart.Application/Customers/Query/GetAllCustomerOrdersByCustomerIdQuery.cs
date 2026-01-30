

using DashMart.Application.Abstraction;
using DashMart.Application.CurrentUserService;
using DashMart.Application.Orders.DTOs;
using DashMart.Application.Orders.Query.Interface;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.xCustomer;
using DashMart.Domain.People.Users;
using MediatR;

namespace DashMart.Application.Customers.Query
{
    public record GetAllCustomerOrdersByCustomerIdQuery
    (
        Guid CustomerId,
        int PageNumber,
        int PageSize
        ) : IRequest<Result<IReadOnlyList<OrderViewDto>>>;


    internal sealed class GetAllCustomerOrdersByCustomerIdQueryHandler
        (ICurrentUserService currentUser, ICustomerRepository customerRepo, IOrderReadRepository orderReadRepo)
        : IRequestHandler<GetAllCustomerOrdersByCustomerIdQuery, Result<IReadOnlyList<OrderViewDto>>>
    {
        public async Task<Result<IReadOnlyList<OrderViewDto>>> Handle(GetAllCustomerOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await customerRepo.GetByPublicIdAsync(request.CustomerId, cancellationToken);

            if (customer == null)
                return Result<IReadOnlyList<OrderViewDto>>.Failure("Customer not found", StatusCodeEnum.NotFound);

            var isOwner = customer.PublicId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.ShowOrders);

            if (!isOwner && !isUser)
                return Result<IReadOnlyList<OrderViewDto>>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            if (pageNumber <= 0)
                pageNumber = ApplicationSettings.DefaultPageNumber;

            if (pageSize <= 0)
                pageSize = ApplicationSettings.DefaultPageSize;


            return Result<IReadOnlyList<OrderViewDto>>.Success(await orderReadRepo.GetAllOrdersByCustomerIdAsync(customer.Id, pageSize, pageNumber, cancellationToken));
            

        }
    }

}
