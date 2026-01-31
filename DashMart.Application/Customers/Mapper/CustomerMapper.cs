
using DashMart.Application.Customers.DTOs;
using DashMart.Domain.People.Customers;
using DashMart.Domain.People.Persons;

namespace DashMart.Application.Customers.Mapper
{
    public static class CustomerMapper
    {
        public static CustomerViewDto? ToCustomerView(Customer? customer)
        {
            if (customer == null) return null;

            bool gender = (customer.Gender == GenderEnum.Female);

            return new CustomerViewDto( 
                PublicId: customer.PublicId,
                FirstName : customer.FirstName, 
                LastName : customer.LastName, 
                Email : customer.Email?.Value, 
                Gender : gender,
                Phone : customer.Phone1.Value,
                Phone2 : customer.Phone2?.Value!
            );
        }


        public static IReadOnlyList<CustomerListDto> ToListOfCustomerListDTO(IList<Customer> customers)
        {
            var list = new List<CustomerListDto>();
            bool gender;

            foreach (Customer customer in customers)
            {
                gender = (customer.Gender == GenderEnum.Female);
                list.Add(new CustomerListDto(customer.FirstName, customer.LastName, customer.Phone1.Value, gender));
            }

            return list;
        }


    }
}
