using DashMart.Application.Abstraction;
using DashMart.Application.CurrentUserService;
using DashMart.Application.Couriers.DTOs;
using DashMart.Application.Couriers.Query.Interface;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Addresses.Streets;
using MediatR;

namespace DashMart.Application.Couriers.Query
{
    public sealed record GetAllCouriersByStreetIdQuery
    (
        Guid StreetId,
        int PageSize,
        int PageNumber
        ) : IRequest<Result<IReadOnlyList<CourierViewDto>>>;

    internal sealed class GetAllCourierByStreetIdQueryHandler
        (ICurrentUserService currentUser, ICourierReadRepository courierReadRepo, IStreetRepository streetRepo) 
        : IRequestHandler<GetAllCouriersByStreetIdQuery, Result<IReadOnlyList<CourierViewDto>>>
    {
        public async Task<Result<IReadOnlyList<CourierViewDto>>> Handle(GetAllCouriersByStreetIdQuery request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.ShowPerson))
                return Result<IReadOnlyList<CourierViewDto>>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var street = await streetRepo.GetStreetDetailsByPublicIdAsync(request.StreetId, cancellationToken);

            if (street == null)
                return Result<IReadOnlyList<CourierViewDto>>.Failure("Street not found", StatusCodeEnum.NotFound);

            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            if (pageNumber <= 0)
                pageNumber = ApplicationSettings.DefaultPageNumber;

            if (pageSize <= 0)
                pageSize = ApplicationSettings.DefaultPageSize;

            return Result<IReadOnlyList<CourierViewDto>>.Success(await courierReadRepo.GetAllCourierByStreetIdAsync(street.Id,pageSize,pageNumber, cancellationToken));

        }
    }


}
