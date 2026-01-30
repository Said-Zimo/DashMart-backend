

using DashMart.Domain.Addresses.Districts;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Addresses.Districts
{
    public sealed class DistrictRepository : IDistrictRepository
    {

        private readonly ApplicationDbContext _context;

        public DistrictRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<District?> GetDistrictDetailsByPublicIdAsync(Guid Id, CancellationToken cancellationToken)
        => await _context.Districts.FirstOrDefaultAsync(x=> x.PublicId == Id, cancellationToken);
    }
}
