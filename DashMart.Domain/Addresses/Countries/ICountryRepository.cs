


namespace DashMart.Domain.Addresses.Countries
{
    public interface ICountryRepository
    {
        Task<Country?> GetCountryDetailsByPublicIdAsync(Guid Id, CancellationToken cancellationToken);

    }
}
