
using DashMart.Domain.Abstraction;
using DashMart.Domain.Products;

namespace DashMart.Domain.Orders
{
    public sealed class OrderItem 
    {
        public int OrderId { get; private set; }
        public Order Order { get; private set; } = default!;
        public int ProductId { get; private set; }
        public Product Product { get; private set; } = default!;
        public decimal Price { get; private set; }
        public short Quantity { get; private set; }
        public decimal TotalLinePrice => Price * Quantity;


        internal OrderItem(int productId, short quantity, decimal price)
        {
            if (quantity <= 0) throw new DomainException("Quantity must be positive");
            if (price <= 0) throw new DomainException("Price must be positive");
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        private OrderItem() { }

        
        public void InsertQuantity(short amount)
        {
            if (amount <= 0) throw new DomainException("Quantity cannot be zero or negative. Remove the item instead");
            Quantity += amount;
        }

        public void ReducingQuantity(short amount)
        {
            if (amount <= 0) throw new DomainException("Quantity cannot be zero or negative. Remove the item instead");
            if (amount >= Quantity) throw new DomainException("Quantity cannot be zero or negative");

            Quantity -= amount;
        }

    }
}
