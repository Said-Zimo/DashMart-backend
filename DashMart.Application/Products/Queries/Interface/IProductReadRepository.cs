using DashMart.Application.Products.DTOs;

namespace DashMart.Application.Products.Queries.Interface
{
    public interface IProductReadRepository
    {
        Task<ProductViewDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ProductListDTO>> ListAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ProductListDTO>> ListByCategoryIdAsync(Guid Id, int pageSize, int pageNumber, CancellationToken cancellationToken = default);

    }
}
