


namespace DashMart.Domain.Addresses.Streets
{
    public interface IStreetRepository
    {
        Task<Street?> GetStreetDetailsByPublicIdAsync(Guid Id, CancellationToken cancellationToken);
    }
}
