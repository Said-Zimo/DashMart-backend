namespace DashMart.Api.Controllers.Product.Requests
{
    public sealed record UpdateProductDetailsRequest(
        string Name,
        string? Description,
        string? HowToUseNote
        );

}
