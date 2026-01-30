
using DashMart.Application.Orders.DTOs;

namespace DashMart.Application.Orders.Query.Interface
{
    public interface IOrderReadRepository
    {
        Task<OrderViewDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<OrderViewDto>> GetAllOrdersAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<OrderViewDto>> GetAllOrdersByCustomerIdAsync(int customerId, int pageSize, int pageNumber, CancellationToken cancellationToken = default);
    }
}
