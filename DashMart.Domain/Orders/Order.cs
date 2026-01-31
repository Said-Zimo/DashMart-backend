

using DashMart.Domain.Abstraction;
using DashMart.Domain.People.Couriers;
using DashMart.Domain.People.Customers;
using DashMart.Domain.Addresses.Streets;
using DashMart.Domain.Validations;

namespace DashMart.Domain.Orders
{
    public sealed class Order : Aggregate
    {
        public int CustomerId { get;private set; }
        public Customer Customer { get;private set; } = default!;
        public int? CourierId { get;private set; }
        public Courier? Courier { get;private set; } 
        public string? Note { get;private set; }
        public int StreetId { get;private set; }
        public Street Street { get;private set; } = default!;
        public string? BuildingNo { get;private set; }
        public string HouseNo { get;private set; } = default!;

        
        public int StatusId { get;private set; }

        public OrderStatusEnum StatusEnum { 
            get => (OrderStatusEnum)StatusId; 
            private set => StatusId = (int)value; 
        }

        public OrderStatus Status { get;private set; } = default!;


        private readonly List<OrderItem> _OrderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _OrderItems;

        private readonly List<OrderStatusLog> _orderStatusLogs = new();
        public IReadOnlyCollection<OrderStatusLog> OrderStatusLogs => _orderStatusLogs;

        public decimal TotalAmount => _OrderItems.Sum(x => x.Price * x.Quantity);

        private Order() { }

        private Order(int customerId, int streetId, string? buildingNo, string houseNo)
        {
            DomainValidation.EnsureValidString(houseNo, 15, "House Number");

            if (buildingNo != null) if (string.IsNullOrWhiteSpace(buildingNo))
                    throw new DomainException("Building Number is not null but is empty"); 
            else BuildingNo = buildingNo;

            if (customerId <= 0)
                throw new DomainException("Customer Id cannot be equal 0 or negative");


            if (streetId <= 0)
                throw new DomainException("Street Id cannot be equal 0 or negative");

            CustomerId = customerId;
            StreetId = streetId;
            HouseNo = houseNo;
            StatusEnum = OrderStatusEnum.Pending ;
        }

        public static Order Create(int customerId , int streetId, string? buildingNo, string houseNo)
        {
            return new Order(customerId,streetId, buildingNo, houseNo);
        }

        public bool IsExistItem(int productId)
        {
            return _OrderItems.Any(x => x.ProductId == productId);
        }

        public void AddItem(int productId, short quantity, decimal price)
        {
            var existingItem = _OrderItems.SingleOrDefault(x => x.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.InsertQuantity(quantity);
            }
            else
            {
                _OrderItems.Add(new OrderItem(productId, quantity, price));
            }
        }

        public void DeleteItem(int productId)
        {
            var item = _OrderItems.SingleOrDefault(x => x.ProductId == productId) ?? throw new DomainException("Item not found");
            _OrderItems.Remove(item);
        }

        public void InsertItemQuantity(int productId, short quantity)
        {
            if (quantity <= 0) throw new DomainException("Quantity cannot be equal 0 or negative");

            var item = _OrderItems.SingleOrDefault(x => x.ProductId == productId) ?? throw new DomainException("Item not found");

            item.InsertQuantity(quantity);
        }

        public void ReducingItemQuantity(int productId, short quantity)
        {
            if (quantity < 0) throw new DomainException("Quantity cannot be negative");

            var item = _OrderItems.SingleOrDefault(x => x.ProductId == productId) ?? throw new DomainException("Item not found");

            var result = item.Quantity - quantity;

            if(result <= 0)
                _OrderItems.Remove(item);
            else
                item.ReducingQuantity((short)result);
        }

        public void SetCourier(int courierId)
        {
            if (CourierId == null) CourierId = courierId; else throw new DomainException("This order already have a courier");

            if (StatusEnum == OrderStatusEnum.Cancelled) throw new DomainException("Cannot set courier for cancelled order");
            if (StatusEnum == OrderStatusEnum.Delivered) throw new DomainException("Cannot set courier for delivered order");

            CourierId = courierId;
            ChangeOrderStatus(DateTime.Now, OrderStatusEnum.CourierAssigned);

        }

        public void SetNote(string note)
        {
            DomainValidation.EnsureValidString(note, 50, "Order Note");

            Note = note;
        }

        private static readonly Dictionary<OrderStatusEnum, HashSet<OrderStatusEnum>> _allowedTransitions = new()
        {
            {
                OrderStatusEnum.Pending,
                new() { OrderStatusEnum.Confirmed, OrderStatusEnum.Cancelled }
            },
            {
                OrderStatusEnum.Confirmed,
                new() { OrderStatusEnum.ReadyForPickup, OrderStatusEnum.Cancelled }
            },
            {
                OrderStatusEnum.ReadyForPickup,
                new() { OrderStatusEnum.CourierAssigned, OrderStatusEnum.Cancelled }
            },
            {
                OrderStatusEnum.CourierAssigned,
                new() { OrderStatusEnum.PickedUp }
            },
            {
                OrderStatusEnum.PickedUp,
                new() { OrderStatusEnum.OutForDelivery }
            },
            {
                OrderStatusEnum.OutForDelivery,
                new() { OrderStatusEnum.Delivered, OrderStatusEnum.DeliveryFailed }
            },
            {
                OrderStatusEnum.DeliveryFailed,
                new() { OrderStatusEnum.OutForDelivery, OrderStatusEnum.Returned }
            },
            {
                OrderStatusEnum.Cancelled,
                new() { OrderStatusEnum.Pending, OrderStatusEnum.Confirmed, OrderStatusEnum.ReadyForPickup, OrderStatusEnum.DeliveryFailed, OrderStatusEnum.Returned }
            }
        };

        public bool IsValidOrderStatusTransition(OrderStatusEnum from, OrderStatusEnum to)
        {
            return _allowedTransitions.TryGetValue(from, out var current) && (current.Contains(to));
        }

        public void ChangeOrderStatus(DateTime date, OrderStatusEnum to)
        {
            if (StatusEnum == to) return;
            if (!IsValidOrderStatusTransition(StatusEnum, to)) 
                throw new DomainException($"Failed order status transition from {StatusEnum} to {to}");

            _orderStatusLogs.Add(OrderStatusLog.Log(Id, to, date, DateTime.Now));
            StatusEnum = to;
            Status = new OrderStatus(to);
        }

    }
}
