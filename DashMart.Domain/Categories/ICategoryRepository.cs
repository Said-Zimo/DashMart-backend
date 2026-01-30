


namespace DashMart.Domain.Categories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Category?> GetByPublicIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsExistAsync(string name, CancellationToken cancellationToken = default);

        void Add(Category entity);

        void Delete(Category entity);

        Task<int> GetInternalIdByPublicIdAsync(Guid PublicId, CancellationToken cancellationToken = default);
    }
}
