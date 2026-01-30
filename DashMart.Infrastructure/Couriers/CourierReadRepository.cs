using DashMart.Application.Couriers.DTOs;
using DashMart.Application.Couriers.Mapper;
using DashMart.Application.Couriers.Query.Interface;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Couriers
{
    public sealed class CourierReadRepository : ICourierReadRepository
    {
        private readonly ApplicationDbContext _context;

        public CourierReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<CourierViewDto>> GetAllCourierByStreetIdAsync(int streetId, int pageSize, int pageNumber, CancellationToken cancellationToken = default)
        => CourierMapper.ToListOfCourierViewDTO(await _context.Couriers
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken));


        public async Task<CourierViewDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => CourierMapper.ToCourierViewDTO(await _context.Couriers.AsNoTracking().FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken));

        public async Task<IReadOnlyList<CourierViewDto>> ListAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default)
        => CourierMapper.ToListOfCourierViewDTO(await _context.Couriers
            .AsNoTracking()
            .Take((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken));



    }
}
