

using System.Threading;

namespace DashMart.Domain.People.Couriers.DriverLicenses
{
    public interface IDriverLicenseRepository
    {
        Task <bool> IsExistByLicenseNumber(string licenseNumber, CancellationToken cancellationToken);
        Task<DriverLicense?> GetDriverLicenseByPublicIdAsync(string licenseNumber, CancellationToken cancellationToken);

    }
}
