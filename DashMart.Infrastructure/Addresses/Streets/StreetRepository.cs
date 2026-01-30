

using DashMart.Domain.Addresses.Streets;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Addresses.Streets
{
    public sealed class StreetRepository : IStreetRepository
    {
        private readonly ApplicationDbContext _context;

        public StreetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Street?> GetStreetDetailsByPublicIdAsync(Guid Id, CancellationToken cancellationToken)
        => await _context.Streets.FirstOrDefaultAsync(st => st.PublicId == Id, cancellationToken);
    }
}
