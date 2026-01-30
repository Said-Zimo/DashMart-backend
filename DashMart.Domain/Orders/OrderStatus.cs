

using DashMart.Domain.Abstraction;

namespace DashMart.Domain.Orders
{
    public sealed class OrderStatus : Entity
    {
        public string Name { get; private set; } = default!;

        private OrderStatus() { }

        public OrderStatus(OrderStatusEnum orderStatus)
        {
            Id = (int)orderStatus;
            Name = orderStatus.ToString();
        }

    }
}
