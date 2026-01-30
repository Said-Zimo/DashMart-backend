using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.UnitOfWorks;
using MediatR;

namespace DashMart.Application.Users.Command
{
    public sealed record DeleteUserCommand
    (
        Guid UserId
        ) : IRequest<Result<string>>;


    internal sealed class DeleteUserCommandHandler
        (ICurrentUserService currentUser, IUserRepository userRepo, IUnitOfWork unitOfWork): IRequestHandler<DeleteUserCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.DeletePerson))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var user = await userRepo.GetByPublicIdAsync(request.UserId, cancellationToken);

            if (user == null) return Result<string>.Failure("User not found", StatusCodeEnum.NotFound);

            userRepo.Delete(user);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("User deleted successfully");

        }
    }

}
