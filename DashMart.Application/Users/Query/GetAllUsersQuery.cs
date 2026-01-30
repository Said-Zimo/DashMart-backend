using DashMart.Application.Abstraction;
using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Application.Users.DTOs;
using DashMart.Application.Users.Query.Interface;
using DashMart.Domain.People.Users;
using MediatR;

namespace DashMart.Application.Users.Query  
{
    public sealed record GetAllUsersQuery
    (int PageSize, int PageNumber) : IRequest<Result<IReadOnlyList<UserViewDto>>>;


    internal sealed class GetAllUsersQueryHandler
        (ICurrentUserService currentUser, IUserReadRepository userReadRepo): IRequestHandler<GetAllUsersQuery, Result<IReadOnlyList<UserViewDto>>>
    {
        public async Task<Result<IReadOnlyList<UserViewDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.ShowPerson))
                return Result<IReadOnlyList<UserViewDto>>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            if (pageNumber <= 0) 
                pageNumber = ApplicationSettings.DefaultPageNumber;
            
            if(pageSize <= 0)
                pageSize = ApplicationSettings.DefaultPageSize;

            return Result<IReadOnlyList<UserViewDto>>.Success(await userReadRepo.ListAllAsync(pageSize, pageNumber, cancellationToken));

        }
    }
}
