using DashMart.Domain.Abstraction;
using DashMart.Domain.People.Persons;
using DashMart.Domain.Orders;

namespace DashMart.Domain.People.Customers
{
    public sealed class Customer : Person
    {
        public decimal Balance { get; private set; }
        private readonly List<Order> _orders = new();

        public IReadOnlyCollection<Order> Orders => _orders;

        private Customer() { }

        private Customer(string firstName, string lastName,string passwordHash, Phone phone1, Phone? phone2, Email? email, GenderEnum gender,RoleEnum role, PersonAddressDetails personAddress) 
            : base(firstName, lastName, passwordHash, phone1, phone2, email, gender, role, personAddress)
        {
        }

        public static Customer Create(string firstName, string lastName,string passwordHash, string phone,
            string? phone2,string? email, GenderEnum gender, RoleEnum role, PersonAddressDetails address)
        {
            var mainPhone = new Phone(phone);

            Phone otherPhone;

            if (!string.IsNullOrWhiteSpace(phone2))
                otherPhone = new Phone(phone2);
            else
                otherPhone = null!;

            Email customerEmail;

            if (!string.IsNullOrWhiteSpace(email))
                customerEmail = new Email(email);
            else
                customerEmail = null!;

            return new Customer(firstName, lastName, passwordHash, mainPhone, otherPhone, customerEmail, gender, role, address);
        }

        public void PlaceOrder(Order order)
        {
            if (!IsActive) throw new DomainException($"This customer with Id {PublicId} is not active");

            if (order == null)
                throw new DomainException("Order cannot be null");

            if (Balance < order.TotalAmount)
                throw new DomainException("Amount does not enough");

            Balance -= order.TotalAmount;

            _orders.Add(order);
        } 


    }


}
