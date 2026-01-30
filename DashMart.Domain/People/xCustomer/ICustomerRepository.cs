

namespace DashMart.Domain.People.xCustomer
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Customer?> GetByPublicIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
        Task<bool> IsExistsByPhoneNumber(string phoneNumber, CancellationToken cancellationToken = default);

        void Add(Customer entity);
        void Delete(Customer entity);
    }
}
