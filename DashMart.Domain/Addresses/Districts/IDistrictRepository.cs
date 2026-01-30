
using DashMart.Domain.Addresses.Streets;

namespace DashMart.Domain.Addresses.Districts
{
    public interface IDistrictRepository
    {
        Task<District?> GetDistrictDetailsByPublicIdAsync(Guid Id, CancellationToken cancellationToken);

    }
}
