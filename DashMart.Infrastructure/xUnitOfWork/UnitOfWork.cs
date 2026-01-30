using DashMart.Domain.UnitOfWorks;
using DashMart.Infrastructure.Context;


namespace DashMart.Infrastructure.xUnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangeAsync(CancellationToken cancellationToken)
        => await _context.SaveChangesAsync(cancellationToken);
    }
}
