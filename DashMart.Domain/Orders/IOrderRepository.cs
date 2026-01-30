

namespace DashMart.Domain.Orders
{
    public interface IOrderRepository 
    {
        Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Order?> GetByPublicIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Add(Order entity);
        void Delete(Order entity);

    }
}
