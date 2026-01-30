using DashMart.Application.Abstraction;
using DashMart.Application.CurrentUserService;
using DashMart.Application.Customers.DTOs;
using DashMart.Application.Customers.Query.Interface;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using MediatR;

namespace DashMart.Application.Customers.Query
{
    public sealed record GetAllCustomersQuery
    (int PageSize, int PageNumber) : IRequest<Result<IReadOnlyList<CustomerListDto>>>;

    internal sealed class GetAllCustomersQueryHandler
        (ICurrentUserService currentUser, ICustomerReadRepository customerReadRepo) 
        : IRequestHandler<GetAllCustomersQuery, Result<IReadOnlyList<CustomerListDto>>>
    {
        public async Task<Result<IReadOnlyList<CustomerListDto>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.ShowPerson))
                return Result<IReadOnlyList<CustomerListDto>>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            if (pageNumber <= 0)
                pageNumber = ApplicationSettings.DefaultPageNumber;

            if (pageSize <= 0)
                pageSize = ApplicationSettings.DefaultPageSize;

            return Result<IReadOnlyList<CustomerListDto>>.Success(await customerReadRepo.ListAllAsync(pageSize, pageNumber, cancellationToken));

        }
    }

}
