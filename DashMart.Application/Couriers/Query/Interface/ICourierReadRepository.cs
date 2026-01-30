using DashMart.Application.Couriers.DTOs;

namespace DashMart.Application.Couriers.Query.Interface
{
    public interface ICourierReadRepository
    {
        Task<CourierViewDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<CourierViewDto>> GetAllCourierByStreetIdAsync(int streetId, int pageSize, int pageNumber, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<CourierViewDto>> ListAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default);
    }
}
