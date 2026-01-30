


namespace DashMart.Domain.Carts
{
    public interface ICartRepository 
    {
        Task<Cart?> GetByPublicIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Cart?> GetActiveShippingCartByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
        Task<bool> IsExistCart(int customer, CancellationToken cancellationToken = default);
        void Add(Cart entity);
        void Delete(Cart entity);
    }
}
