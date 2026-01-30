

namespace DashMart.Domain.Abstraction
{
    public sealed class DomainException : Exception
    {
        public DomainException(string? message) : base(message)
        {
        }
    }
}
