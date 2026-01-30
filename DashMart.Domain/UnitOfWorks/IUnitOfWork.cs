

namespace DashMart.Domain.UnitOfWorks

{
    public interface IUnitOfWork
    {
        Task SaveChangeAsync(CancellationToken cancellationToken);
    }
}
