
namespace DashMart.Application.Categories.Query.Interface
{
    public interface ICategoryReadRepository
    {
        Task<GetCategoryByIdQueryResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<GetAllCategoryQueryResponse>> GetAllCategoriesAsync(int pageSize, int pageNumber,CancellationToken cancellationToken = default);
    }
}
