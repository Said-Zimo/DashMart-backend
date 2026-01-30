

using DashMart.Domain.Addresses.Cities;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Addresses.Cities
{
    public sealed class CityRepository : ICityRepository
    {

        private readonly ApplicationDbContext _context;

        public CityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<City?> GetCityDetailsByPublicIdAsync(Guid Id, CancellationToken cancellationToken)
        => await _context.Cities.FirstOrDefaultAsync(c => c.PublicId == Id, cancellationToken);
    }
}
