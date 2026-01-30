using DashMart.Application.Abstraction;
using DashMart.Application.CurrentUserService;
using DashMart.Application.Categories.Query.Interface;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using MediatR;

namespace DashMart.Application.Categories.Query
{

    public sealed record GetAllCategoryQueryResponse
    (string name);

    public sealed record GetAllCategoryQuery
    (int PageSize, int PageNumber) : IRequest<Result<IReadOnlyList<GetAllCategoryQueryResponse>>>;


    internal sealed class GetAllCategoryQueryHandler
        (ICategoryReadRepository categoryRepo) : IRequestHandler<GetAllCategoryQuery,Result<IReadOnlyList< GetAllCategoryQueryResponse>>>
    {
        public async Task<Result<IReadOnlyList<GetAllCategoryQueryResponse>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            if (pageNumber <= 0)
                pageNumber = ApplicationSettings.DefaultPageNumber;

            if (pageSize <= 0)
                pageSize = ApplicationSettings.DefaultPageSize;

            return Result<IReadOnlyList<GetAllCategoryQueryResponse>>.Success(await categoryRepo.GetAllCategoriesAsync( pageSize, pageNumber, cancellationToken));

        }
    }

}
