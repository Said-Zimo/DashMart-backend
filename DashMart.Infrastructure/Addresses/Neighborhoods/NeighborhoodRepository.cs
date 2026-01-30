

using DashMart.Domain.Addresses.Neighborhoods;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Addresses.Neighborhoods
{
    public sealed class NeighborhoodRepository : INeighborhoodRepository
    {
        private readonly ApplicationDbContext _context;

        public NeighborhoodRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Neighborhood?> GetNeighborhoodDetailsByPublicIdAsync(Guid Id, CancellationToken cancellationToken)
        => await _context.Neighborhoods.FirstOrDefaultAsync(x=> x.PublicId == Id, cancellationToken);
    }
}
