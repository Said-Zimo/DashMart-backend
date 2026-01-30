
using DashMart.Domain.Abstraction;

namespace DashMart.Domain.Orders
{
    public sealed class OrderStatusLog : Entity
    {
        public int OrderId { get;private set; }
        public Order Order { get; private set; } = default!;
        public int StatusId { get; private set; }
        public OrderStatusEnum StatusEnum { 
            get => (OrderStatusEnum)StatusId ; 
            private set => StatusId = (int)value; 
        }
        public OrderStatus Status { get; private set; } = default!;
        public DateTime Date {  get; private set; }
        public string? Note { get; private set; }

        public OrderStatusLog() { }

        private OrderStatusLog(int orderID, OrderStatusEnum newStatus, DateTime date, DateTime currentDate, string? note = null)
        {
            OrderId = orderID;
            StatusEnum = newStatus;
            if (date > currentDate) throw new DomainException("Date cannot be in future");
            Date = date;
            Note = note;
        }

        public static OrderStatusLog Log(int orderId, OrderStatusEnum statusId, DateTime date, DateTime currentDate, string? note = null)
        {
            return new OrderStatusLog(orderId, statusId, date, currentDate,note);
        }
    }
}
