

namespace DashMart.Domain.People.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<User?> GetByPublicIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
        Task<bool> IsExistByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);

        void Add(User entity);

        void Delete(User entity);
    }
}
