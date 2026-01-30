using DashMart.Application.Abstraction;
using DashMart.Application.CurrentUserService;
using DashMart.Application.Carts.Query.Interface;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using MediatR;

namespace DashMart.Application.Carts.Query
{

    public sealed record GetAllCartsQueryResponse
    (
        Guid CustomerId,
        decimal TotalAmount
        );


    public sealed record GetAllCartsQuery
    (int PageSize, int PageNumber) : IRequest<Result<IReadOnlyList<GetAllCartsQueryResponse>>>;


    internal sealed class GetAllCartsQueryHandler
        (ICurrentUserService currentUser, ICartReadRepository cartReadRepo)
        : IRequestHandler<GetAllCartsQuery, Result<IReadOnlyList<GetAllCartsQueryResponse>>>
    {
        public async Task<Result<IReadOnlyList<GetAllCartsQueryResponse>>> Handle(GetAllCartsQuery request, CancellationToken cancellationToken)
        {

            if (!currentUser.HasPermission(UserPermissionsEnum.AccessCart))
                return Result<IReadOnlyList<GetAllCartsQueryResponse>>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            if (pageNumber <= 0)
                pageNumber = ApplicationSettings.DefaultPageNumber;

            if (pageSize <= 0)
                pageSize = ApplicationSettings.DefaultPageSize;

            return Result<IReadOnlyList<GetAllCartsQueryResponse>>.Success(await cartReadRepo.GetCartsAllAsync(pageSize, pageNumber, cancellationToken));

        }
    }

}
