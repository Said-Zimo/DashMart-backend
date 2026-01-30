

using DashMart.Domain.Carts;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.ShippingCarts
{
    public sealed class ShippingCartRepository : ICartRepository
    {

        private readonly ApplicationDbContext _context;

        public ShippingCartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Cart cart) => _context.Carts.Add(cart);
        

        public void Delete(Cart cart)=> _context.Carts.Remove(cart);

        public async Task<Cart?> GetActiveShippingCartByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
        => await _context.Carts.FirstOrDefaultAsync(x=> x.CustomerId == customerId ,cancellationToken);

        public async Task<Cart?> GetByPublicIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Carts.FirstOrDefaultAsync(x=> x.PublicId == id ,cancellationToken);

        public async Task<bool> IsExistCart(int customerId, CancellationToken cancellationToken = default)
        => await _context.Carts.AnyAsync(x => x.CustomerId == customerId, cancellationToken);
    }
}
