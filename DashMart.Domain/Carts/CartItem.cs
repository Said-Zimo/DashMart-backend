

using DashMart.Domain.Abstraction;
using DashMart.Domain.Products;

namespace DashMart.Domain.Carts
{
    public sealed class CartItem 
    {
        public int CartId { get; private set; }
        public Cart Cart { get; private set; } = default!;
        public int ProductId { get; private set; }
        public Product Product { get; private set; } = default!;
        public short Quantity { get; private set; }
        public decimal Price { get; private set; }
        public decimal TotalLinePrice => Price * Quantity;

        private CartItem() { }

        internal CartItem(int productId, short quantity, decimal price)
        {
            if (quantity <= 0) throw new DomainException("Unable quantity to be negative");
            if (price <= 0) throw new DomainException("Unable price to be negative");
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        public static CartItem Create(int  productId, short quantity, decimal price)
        {
            return new CartItem(productId, quantity, price);
        }

        public decimal CalculateTotalPrice()
        {
            if (Product == null)
                throw new DomainException("Product data is not loaded to calculate price.");
            
            return Price * Quantity;
        }

        public void IncreaseQuantity(short amount)
        {
            if (amount <= 0) throw new DomainException("Increase amount must be positive");
            Quantity += amount;
        }

        public void DecreaseQuantity(short amount)
        {
            if (amount <= 0) throw new DomainException("Increase amount must be positive");

            if (Quantity - amount <= 0 ) 
                throw new DomainException("Quantity cannot be zero or negative. Remove the item instead");

            Quantity -= amount;
        }

        public void UpdateQuantity(short amount)
        {
            if (amount <= 0) throw new DomainException("Amount must be positive");

            Quantity = amount;
        }

    }
}
