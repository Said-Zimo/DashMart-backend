using DashMart.Application.CurrentUserService;
using DashMart.Application.Customers.DTOs;
using DashMart.Application.Customers.Query.Interface;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Users;
using MediatR;

namespace DashMart.Application.Customers.Query
{
    public sealed record GetCustomerQuery
    (
        Guid CustomerId
        ) : IRequest<Result<CustomerViewDto?>>;


    internal sealed class GetCustomerQueryHandler
        (ICurrentUserService currentUser, ICustomerReadRepository customerReadRepo) : IRequestHandler<GetCustomerQuery, Result<CustomerViewDto?>>
    {
        public async Task<Result<CustomerViewDto?>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await customerReadRepo.GetByIdAsync(request.CustomerId, cancellationToken);

            if (customer == null)
                return Result<CustomerViewDto?>.Failure("Customer not found", StatusCodeEnum.NotFound);

            var isOwner = customer.PublicId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.ShowPerson);

            if (!isOwner && !isUser)
                return Result<CustomerViewDto?>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            return Result<CustomerViewDto?>.Success(customer);
        }
    }

}
