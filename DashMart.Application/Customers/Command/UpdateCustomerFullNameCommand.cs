using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Customers;
using DashMart.Domain.People.Users;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Customers.Command
{
    public sealed record UpdateCustomerFullNameCommand
    (
        Guid CustomerId,
        string FirstName,
        string LastName
        ) : IRequest<Result<string>>;

    public sealed class UpdateCustomerFullNameCommandValidator : AbstractValidator<UpdateCustomerFullNameCommand>
    {
        public UpdateCustomerFullNameCommandValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("Fist name cannot be empty or null")
                .MaximumLength(30).WithMessage("First name length must be less than 31 character");

            RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Last name cannot be empty or null")
                .MaximumLength(30).WithMessage("Last name length must be less than 31 character");
        }
    }

    internal sealed class UpdateCustomerFullNameCommandHandler
        (ICurrentUserService currentUser, ICustomerRepository customerRepo, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCustomerFullNameCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateCustomerFullNameCommand request, CancellationToken cancellationToken)
        {

            var customer = await customerRepo.GetByPublicIdAsync(request.CustomerId);

            if (customer == null) return Result<string>.Failure("Customer not found", StatusCodeEnum.NotFound);

            var isOwner = customer.PublicId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.UpdatePerson);

            if (!isOwner && !isUser)
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            customer.UpdateFirstName(request.FirstName);
            customer.UpdateLastName(request.LastName);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Customer full name updated successfully");
        }
    }

}
