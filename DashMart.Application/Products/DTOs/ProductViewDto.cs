

namespace DashMart.Application.Products.DTOs
{
    public sealed record ProductViewDto
    {
        public string Name { get; init; } = default!;
        public string? Description { get; init; }
        public int Grams { get; init; }
        public decimal Price { get; init; }
        public string? HowToUseNote { get; init; }
        public string SKU { get; init; } = default!;
    }
}
