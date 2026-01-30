namespace DashMart.Api.Controllers.Customer.Requests
{
    public sealed record UpdateCustomerFullNameRequest(string FirstName, string LastName);

    public sealed record PlaceCustomerOrderRequest(string? Note);

    public sealed record AddCustomerAddressRequest
    (
        string AddressTitle,
        ushort AddressType,
        string HouseNumber,
        string? BuildingNo = null
        );
}
