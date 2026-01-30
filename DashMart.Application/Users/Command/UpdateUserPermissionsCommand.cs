using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.UnitOfWorks;
using MediatR;

namespace DashMart.Application.Users.Command
{
    public sealed record UpdateUserPermissionsCommand
    (
        Guid UserId,
        int Permissions
        ) : IRequest<Result<string>>;


    internal sealed class UpdateUserPermissionsCommandHandler
        (ICurrentUserService currentUser, IUserRepository userRepo, IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserPermissionsCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateUserPermissionsCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdatePerson))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var user = await userRepo.GetByPublicIdAsync(request.UserId,  cancellationToken);

            if (user == null) return Result<string>.Failure("User not found", StatusCodeEnum.NotFound);

            if (!Enum.IsDefined(typeof(UserPermissionsEnum), request.Permissions))
                return Result<string>.Failure("Permissions is not valid", StatusCodeEnum.BadRequest);

            user.AddNewPermissions((UserPermissionsEnum)request.Permissions);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("User permissions updated successfully");

        }
    }

}
