

using DashMart.Domain.Addresses.Countries;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Addresses.Countries
{
    public sealed class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDbContext _context;

        public CountryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Country?> GetCountryDetailsByPublicIdAsync(Guid Id, CancellationToken cancellationToken)
        => await _context.Countries.FirstOrDefaultAsync(x=> x.PublicId == Id, cancellationToken);
    }
}
