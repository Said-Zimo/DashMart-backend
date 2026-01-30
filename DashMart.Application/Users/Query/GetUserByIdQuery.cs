using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Application.Users.DTOs;
using DashMart.Application.Users.Query.Interface;
using DashMart.Domain.People;
using DashMart.Domain.People.Users;
using MediatR;

namespace DashMart.Application.Users.Query
{
    public sealed record GetUserByIdQuery
    (
        Guid UserId
        ) : IRequest<Result<UserViewDto?>>;



    internal sealed class GetUserByIdQueryHandler
        (ICurrentUserService currentUser, IUserReadRepository userReadRepo) : IRequestHandler<GetUserByIdQuery, Result<UserViewDto?>>
    {
        public async Task<Result<UserViewDto?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userReadRepo.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
                return Result<UserViewDto?>.Failure("User not found", StatusCodeEnum.NotFound);

            var isOwner = user.UserId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.ShowPerson);

            if (!isOwner && !isUser)
                return Result<UserViewDto?>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            return Result<UserViewDto?>.Success(user);

        }
    }
}
