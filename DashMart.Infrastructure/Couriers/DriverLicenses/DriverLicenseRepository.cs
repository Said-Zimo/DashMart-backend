

using DashMart.Domain.People.Couriers.DriverLicenses;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Couriers.DriverLicenses
{
    public sealed class DriverLicenseRepository : IDriverLicenseRepository
    {
        private readonly ApplicationDbContext _context;

        public DriverLicenseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DriverLicense?> GetDriverLicenseByPublicIdAsync(string licenseNumber, CancellationToken cancellationToken)
        => await _context.DriverLicenses.FirstOrDefaultAsync(x => x.LicenseNumber == licenseNumber);

        public async Task<bool> IsExistByLicenseNumber(string licenseNumber, CancellationToken cancellationToken)
        => await _context.DriverLicenses.AnyAsync(x=> x.LicenseNumber == licenseNumber); 
    }
}
