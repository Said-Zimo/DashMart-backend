

using DashMart.Application.Orders.DTOs;
using DashMart.Application.Orders.Mapper;
using DashMart.Application.Orders.Query.Interface;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Orders
{
    public sealed class OrderReadRepository : IOrderReadRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<OrderViewDto>> GetAllOrdersAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default)
       => OrderMapper.ToListOfOrderViewDTO(await _context.Orders
           .AsNoTracking()
           .Skip((pageNumber -1 ) * pageSize)
           .Take(pageSize)
           .ToListAsync(cancellationToken));

        public async Task<IReadOnlyList<OrderViewDto>> GetAllOrdersByCustomerIdAsync(int customerId, int pageSize, int pageNumber, CancellationToken cancellationToken = default)
        => OrderMapper.ToListOfOrderViewDTO( await _context.Orders
            .AsNoTracking()
            .Skip((pageNumber -1 ) * pageSize)
            .Take(pageSize)
            .Where(x => x.CustomerId == customerId)
            .ToListAsync(cancellationToken));

        public async Task<OrderViewDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => OrderMapper.ToOrderViewDTO(await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken));

    }
}
