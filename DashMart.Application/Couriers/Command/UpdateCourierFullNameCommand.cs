using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Couriers;
using DashMart.Domain.People.xCustomer;
using DashMart.Domain.People.Users;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Couriers.Command
{
    public sealed record UpdateCourierFullNameCommand
    (
        Guid CourierId,
        string FirstName,
        string LastName
        ) : IRequest<Result<string>>;

    public sealed class UpdateCourierFullNameCommandValidator : AbstractValidator<UpdateCourierFullNameCommand>
    {
        public UpdateCourierFullNameCommandValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("Fist name cannot be empty or null")
                .MaximumLength(30).WithMessage("First name length must be less than 31 character");

            RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Last name cannot be empty or null")
                .MaximumLength(30).WithMessage("Last name length must be less than 31 character");
        }
    }

    internal sealed class UpdateCourierFullNameCommandHandler
        (ICurrentUserService currentUser, ICourierRepository courierRepo, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCourierFullNameCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateCourierFullNameCommand request, CancellationToken cancellationToken)
        {
            var courier = await courierRepo.GetByPublicIdAsync(request.CourierId, cancellationToken);

            if (courier == null) return Result<string>.Failure("Courier not found", StatusCodeEnum.NotFound);

            var isOwner = courier.PublicId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.UpdatePerson);

            if (!isOwner && !isUser)
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            courier.UpdateFirstName(request.FirstName);
            courier.UpdateLastName(request.LastName);

            await unitOfWork.SaveChangeAsync(cancellationToken);
            
            return Result<string>.Success("Courier full name updated successfully");


        }
    }



}
