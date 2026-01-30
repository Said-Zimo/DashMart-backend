
namespace DashMart.Domain.Abstraction
{
    public abstract class Aggregate : Entity
    {
        public byte[] RowVersion { get; private set; } = default!;
    }
}
