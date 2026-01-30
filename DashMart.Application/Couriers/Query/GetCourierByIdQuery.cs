using DashMart.Application.CurrentUserService;
using DashMart.Application.Couriers.DTOs;
using DashMart.Application.Couriers.Query.Interface;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Couriers;
using DashMart.Domain.People.Users;
using MediatR;

namespace DashMart.Application.Couriers.Query
{
    public sealed record GetCourierByIdQuery
    (
        Guid CourierId
        ) : IRequest<Result<CourierViewDto?>>;

    internal sealed class GetCourierByIdQueryHandler
        (ICurrentUserService currentUser, ICourierReadRepository courierReadRepo) : IRequestHandler<GetCourierByIdQuery, Result<CourierViewDto?>>
    {
        public async Task<Result<CourierViewDto?>> Handle(GetCourierByIdQuery request, CancellationToken cancellationToken)
        {

            var courier = await courierReadRepo.GetByIdAsync(request.CourierId, cancellationToken);

            if (courier == null) return Result<CourierViewDto?>.Failure("Courier not found", StatusCodeEnum.NotFound);

            var isOwner = courier.PublicId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.ShowPerson);

            if (!isOwner && !isUser)
                return Result<CourierViewDto?>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            return Result<CourierViewDto?>.Success(courier);
        }
    }


}
