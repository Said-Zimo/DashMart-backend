using DashMart.Application.Abstraction;
using DashMart.Application.CurrentUserService;
using DashMart.Application.Orders.DTOs;
using DashMart.Application.Orders.Query.Interface;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using MediatR;

namespace DashMart.Application.Orders.Query
{
    public sealed record GetAllOrdersQuery
    (int PageSize, int PageNumber) : IRequest<Result<IReadOnlyList<OrderViewDto>>>;

    internal sealed class GetAllOrdersQueryHandler
        (ICurrentUserService currentUser, IOrderReadRepository orderReadRepo): IRequestHandler<GetAllOrdersQuery, Result<IReadOnlyList<OrderViewDto>>>
    {
        public async Task<Result<IReadOnlyList<OrderViewDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.ShowOrders))
                return Result<IReadOnlyList<OrderViewDto>>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            if (pageNumber <= 0)
                pageNumber = ApplicationSettings.DefaultPageNumber;

            if (pageSize <= 0)
                pageSize = ApplicationSettings.DefaultPageSize;

            return Result<IReadOnlyList<OrderViewDto>>.Success(await orderReadRepo.GetAllOrdersAsync(pageSize,pageNumber, cancellationToken));

        }
    }
}
