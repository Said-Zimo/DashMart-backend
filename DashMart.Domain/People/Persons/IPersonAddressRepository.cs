

namespace DashMart.Domain.People.Persons
{
    public interface IPersonAddressRepository
    {
        Task<PersonAddressDetails?> GetAddressDetailsByPublicIdAsync(Guid Id , CancellationToken cancellationToken);
    }
}
