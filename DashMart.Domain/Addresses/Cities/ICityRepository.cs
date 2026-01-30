

namespace DashMart.Domain.Addresses.Cities
{
    public interface ICityRepository
    {
        Task<City?> GetCityDetailsByPublicIdAsync(Guid Id, CancellationToken cancellationToken);

    }
}
