

using DashMart.Application.Customers.DTOs;
using DashMart.Application.Customers.Mapper;
using DashMart.Application.Customers.Query.Interface;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Customers
{
    public sealed class CustomerReadRepository : ICustomerReadRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerViewDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => CustomerMapper.ToCustomerView(await _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken));

        public async Task<IReadOnlyList<CustomerListDto>> ListAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default)
        => CustomerMapper.ToListOfCustomerListDTO(await _context.Customers
            .AsNoTracking()
            .Skip((pageNumber -1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken));

    }
}
