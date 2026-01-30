using DashMart.Application.Categories.Query.Interface;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using MediatR;

namespace DashMart.Application.Categories.Query
{
    public sealed record GetCategoryByIdQueryResponse(string Name);

    public sealed record GetCategoryByIdQuery
    (
        Guid CategoryId
        ) : IRequest<Result< GetCategoryByIdQueryResponse?>>;


    internal sealed class GetCategoryByIdQueryHandler
        (ICategoryReadRepository categoryReadRepo) 
        : IRequestHandler<GetCategoryByIdQuery, Result<GetCategoryByIdQueryResponse?>>
    {
        public async Task<Result<GetCategoryByIdQueryResponse?>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return Result<GetCategoryByIdQueryResponse?>.Success(await categoryReadRepo.GetByIdAsync(request.CategoryId, cancellationToken));
        }
    }

}
