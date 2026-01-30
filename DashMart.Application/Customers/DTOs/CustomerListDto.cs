

namespace DashMart.Application.Customers.DTOs
{
    public sealed record CustomerListDto
    (
        string FirstName,
        string LastName,
        string Phone1,
        bool Gender
        );
}
