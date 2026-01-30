namespace DashMart.Api.Controllers.Product.Requests
{
    public sealed record AddProductImagesRequest(ICollection<string> ImagePaths);
}
