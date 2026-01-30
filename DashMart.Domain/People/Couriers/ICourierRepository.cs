



namespace DashMart.Domain.People.Couriers
{
    public interface ICourierRepository 
    {
        Task<Courier?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Courier?> GetByPublicIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Courier?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
        Task<bool> IsExistsByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);

        void Add(Courier entity);

        void Delete(Courier entity);
    }
}
