using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Couriers;
using DashMart.Domain.People.Users;
using DashMart.Domain.UnitOfWorks;
using MediatR;

namespace DashMart.Application.Couriers.Command
{
    public sealed record DeleteCourierCommand
    (
        Guid CourierId
        ) : IRequest <Result<string>>;


    internal sealed class DeleteCourierCommandHandler
        (ICurrentUserService currentUser, ICourierRepository courierRepo, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCourierCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteCourierCommand request, CancellationToken cancellationToken)
        {
            var courier = await courierRepo.GetByPublicIdAsync( request.CourierId , cancellationToken);

            if (courier == null) return Result<string>.Failure("Courier not found", StatusCodeEnum.NotFound);

            var isOwner = courier.PublicId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.DeletePerson);

            if (!isOwner && !isUser)
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            courierRepo.Delete(courier);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Courier deleted successfully");

        }
    }

}
