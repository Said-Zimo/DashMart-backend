using DashMart.Application.CurrentUserService;
using DashMart.Application.Carts.Query.Interface;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Users;
using MediatR;

namespace DashMart.Application.Carts.Query
{

    public sealed record GetCartByIdQueryResponse
    (
        Guid CustomerId,
        string CustomerFullName,
        decimal TotalAmount
        );

    public sealed record GetCartByIdQuery
    (
        Guid CartId
        ) : IRequest<Result<GetCartByIdQueryResponse?>>;



    internal sealed class GetCartByIdQueryHandler
        (ICurrentUserService currentUser, ICartReadRepository cartReadRepo): IRequestHandler<GetCartByIdQuery, Result<GetCartByIdQueryResponse?>>
    {
        public async Task<Result<GetCartByIdQueryResponse?>> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            var cart = await cartReadRepo.GetByIdAsync(request.CartId, cancellationToken);

            if (cart == null)
                return Result<GetCartByIdQueryResponse?>.Failure("Cart not found", StatusCodeEnum.NotFound);

            var isOwner = cart.CustomerId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.AccessCart);

            if (!isOwner && !isUser)
                return Result<GetCartByIdQueryResponse?>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            return Result<GetCartByIdQueryResponse?>.Success(cart);
        }
    }

}
