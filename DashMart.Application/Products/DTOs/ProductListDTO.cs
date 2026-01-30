

namespace DashMart.Application.Products.DTOs
{
    public sealed record ProductListDTO
    {
        public string Name { get; init; } = default!;
        public int Grams { get; init; }
        public decimal Price { get; init; }
    }
}
