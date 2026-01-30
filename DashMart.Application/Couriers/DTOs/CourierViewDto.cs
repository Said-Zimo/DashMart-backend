


namespace DashMart.Application.Couriers.DTOs
{
    public sealed record CourierViewDto
    (
        Guid PublicId,
        bool IsReadyToWork ,
        string FirstName ,
        string LastName ,
        string Phone1 
        );

    
}
