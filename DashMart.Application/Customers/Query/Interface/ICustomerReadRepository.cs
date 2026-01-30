

using DashMart.Application.Customers.DTOs;

namespace DashMart.Application.Customers.Query.Interface
{
    public interface ICustomerReadRepository
    {
        Task<CustomerViewDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<CustomerListDto>> ListAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default);
    }
}
