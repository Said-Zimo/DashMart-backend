

namespace DashMart.Application.Customers.DTOs
{
    public sealed record CustomerViewDto
    (
        Guid PublicId,
        string FirstName,
        string LastName,
        string Phone,
        string? Phone2,
        string? Email,
        bool Gender
        );
   
}
