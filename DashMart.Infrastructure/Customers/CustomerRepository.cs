

using DashMart.Domain.People.Customers;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Customers
{
    public sealed class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Customer customer) => _context.Customers.Add(customer);

        public void Delete(Customer customer) 
        {
            customer.IsDeleted = true;
            customer.DeletedAt = DateTime.Now;
        }

        public async Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Customers
            .Include(x=> x.Orders)
            .ThenInclude(x=> x.OrderItems)
            .Include(x=> x.Addresses)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        
        public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => await _context.Customers
            .Include(x => x.Orders)
            .ThenInclude(x => x.OrderItems)
            .Include(x => x.Addresses)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x=> x.Phone1.Value == phoneNumber || x.Phone2!.Value == phoneNumber, cancellationToken);

        
        public async Task<Customer?> GetByPublicIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Customers
            .Include(x => x.Orders)
            .ThenInclude(x => x.OrderItems)
            .Include(x => x.Addresses)
            .AsSplitQuery().FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken);

        public async Task<bool> IsExistsByPhoneNumber(string phoneNumber, CancellationToken cancellationToken = default)
        => await _context.Customers.AnyAsync(x=> x.Phone1.Value == phoneNumber || x.Phone2!.Value == phoneNumber, cancellationToken);

        

    }
}
