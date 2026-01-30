


namespace DashMart.Domain.Products
{
    public interface IProductRepository
    {
        void Add(Product product);
        void Delete(Product product);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Product?> GetByPublicIdAsync(Guid Id, CancellationToken cancellationToken);
        Task<bool> IsExistAsync(string sku, CancellationToken cancellationToken);
    }
}
