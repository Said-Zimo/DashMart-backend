


namespace DashMart.Application.Carts.Query.Interface
{
    public interface ICartReadRepository
    {
        Task<GetCartByIdQueryResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<GetAllCartsQueryResponse>> GetCartsAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default);

    }
}
