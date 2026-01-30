

using DashMart.Domain.Abstraction;
using DashMart.Domain.People.xCustomer;

namespace DashMart.Domain.Carts
{
    public sealed class Cart : Aggregate
    {
        public int CustomerId { get; private set; }
        public Customer Customer { get; private set; } = default!;
        public decimal TotalAmount => _CartItems.Sum(x => x.CalculateTotalPrice() * x.Quantity);

        private readonly List<CartItem> _CartItems = new();
        public IReadOnlyCollection<CartItem> CartItems => _CartItems;

        private Cart() { }

        private Cart(int customerId)
        {
            CustomerId = customerId;
        }

        public static Cart Create(int customerId)
        {
            return new Cart(customerId);
        }

        public void AddCartItem(int productId, short quantity, decimal price)
        {
            var existingItem = _CartItems.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                _CartItems.Add(CartItem.Create(productId, quantity,price));
            }
        }

        public void DeleteCartItem(int productId)
        {
            var item = _CartItems.SingleOrDefault(x => x.ProductId == productId)?? throw new DomainException("Item not found");

            _CartItems.Remove(item);
        }


    }
}
