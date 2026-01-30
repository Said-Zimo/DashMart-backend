

using DashMart.Application.Users.DTOs;
using DashMart.Application.Users.Mapper;
using DashMart.Application.Users.Query.Interface;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Users
{
    public sealed class UserReadRepository : IUserReadRepository
    {

        private readonly ApplicationDbContext _context;

        public UserReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserViewDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
         => UserMapper.ToUserViewDTO(await _context.Users.FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken));


        public async Task<IReadOnlyList<UserViewDto>> ListAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default)
        => UserMapper.ToListOfUserViewDTO(await _context.Users
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken));

    }
}
