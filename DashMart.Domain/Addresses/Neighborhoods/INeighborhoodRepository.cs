

namespace DashMart.Domain.Addresses.Neighborhoods
{
    public interface INeighborhoodRepository
    {
        Task<Neighborhood?> GetNeighborhoodDetailsByPublicIdAsync(Guid Id, CancellationToken cancellationToken);

    }
}
