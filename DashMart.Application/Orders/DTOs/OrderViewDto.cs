


namespace DashMart.Application.Orders.DTOs
{
    public sealed record OrderViewDto
    (
         int CustomerId ,
         int? CourierId ,
         string? Note ,
         int StreetId ,
         string? BuildingNo,
         string HouseNo 

        );
}
