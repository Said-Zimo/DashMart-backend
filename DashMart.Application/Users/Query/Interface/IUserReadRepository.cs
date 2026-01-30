

using DashMart.Application.Users.DTOs;

namespace DashMart.Application.Users.Query.Interface
{
    public interface IUserReadRepository
    {
        Task<UserViewDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<UserViewDto>> ListAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default);
    }
}
