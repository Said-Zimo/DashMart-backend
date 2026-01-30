
using DashMart.Domain.Orders;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Orders
{
    public sealed class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Order order) => _context.Orders.Add(order);
        

        public void Delete(Order order)
        {
            order.IsDeleted = true;
            order.DeletedAt = DateTime.Now;
        } 

       
        public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Orders
            .Include(x=> x.OrderItems)
            .ThenInclude(x=> x.Product)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x=> x.Id == id, cancellationToken);

        public async Task<Order?> GetByPublicIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Orders.FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken);
    }
}
