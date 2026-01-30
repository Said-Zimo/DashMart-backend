using DashMart.Application.Carts.Mapper;
using DashMart.Application.Carts.Query;
using DashMart.Application.Carts.Query.Interface;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.ShippingCarts
{
    public sealed class ShippingCartReadRepository : ICartReadRepository
    {

        private readonly ApplicationDbContext _context;

        public ShippingCartReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetCartByIdQueryResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var cart = await _context.Carts.Include(x=> x.Customer).AsNoTracking().FirstOrDefaultAsync(x=> x.PublicId == id, cancellationToken);

            if(cart == null) return null;

            return new GetCartByIdQueryResponse(cart.Customer.PublicId, cart.Customer.FirstName + cart.Customer.FirstName, cart.TotalAmount);
        }

        public async Task<IReadOnlyList<GetAllCartsQueryResponse>> GetCartsAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default)
        => CartMapper.ToCartQueryResponse(await _context.Carts
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync());


    }
}
