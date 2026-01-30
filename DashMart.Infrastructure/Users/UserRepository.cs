
using DashMart.Domain.People.Users;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Users
{
    public sealed class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(User user)
        => _context.Users.Add(user);

        public void Delete(User user)
        {
            user.IsDeleted = true;
            user.DeletedAt = DateTime.Now;
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Users
            .Include(x => x.Addresses)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        
        public Task<User?> GetByPublicIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _context.Users
            .Include(x => x.Addresses)
            .FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken);

        public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => await _context.Users
            .Include(x => x.Addresses)
            .FirstOrDefaultAsync(x => x.Phone1.Value == phoneNumber || x.Phone2!.Value == phoneNumber);

        public async Task<bool> IsExistByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => await _context.Users.AnyAsync(x=> x.Phone1.Value == phoneNumber || x.Phone2!.Value == phoneNumber, cancellationToken);

        
    }
}
