using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.xCustomer;
using DashMart.Domain.People.Users;
using DashMart.Domain.UnitOfWorks;
using MediatR;

namespace DashMart.Application.Customers.Command
{
    public sealed record DeleteCustomerCommand
    (
        Guid CustomerId
        ) : IRequest <Result<string>>;

    internal sealed class DeleteCustomerCommandHandler
        (ICurrentUserService currentUser, ICustomerRepository customerRepo, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCustomerCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await customerRepo.GetByPublicIdAsync(request.CustomerId, cancellationToken);

            if (customer == null) return Result<string>.Failure("Customer not found", StatusCodeEnum.NotFound);

            var isOwner = request.CustomerId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.DeletePerson);

            if (!isOwner && !isUser)
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            customerRepo.Delete(customer);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Customer deleted successfully");

        }
    }

}
