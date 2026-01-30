
using DashMart.Domain.People.Couriers;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Couriers
{
    public sealed class CourierRepository : ICourierRepository
    {
        private readonly ApplicationDbContext _context;

        public CourierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Courier courier) => _context.Couriers.Add(courier);

        public void Delete(Courier courier)
        {
            courier.IsDeleted = true;
            courier.DeletedAt = DateTime.Now;
        } 

        
        public async Task<Courier?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Couriers
            .Include(x => x.Addresses)
            .Include(x=> x.DriverLicenses)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<Courier?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => await _context.Couriers
            .Include(x => x.Addresses)
            .Include(x => x.DriverLicenses)
            .FirstOrDefaultAsync(c => c.Phone1.Value == phoneNumber || c.Phone2!.Value == phoneNumber, cancellationToken);

        public async Task<Courier?> GetByPublicIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Couriers
            .Include(x => x.Addresses)
            .Include(x => x.DriverLicenses)
            .FirstOrDefaultAsync(c => c.PublicId == id, cancellationToken);

        public async Task<bool> IsExistsByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => await _context.Couriers.AnyAsync(x => x.Phone1.Value == phoneNumber || x.Phone2!.Value == phoneNumber, cancellationToken);
    }
}
