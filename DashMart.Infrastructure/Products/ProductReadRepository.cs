

using DashMart.Application.Products.DTOs;
using DashMart.Application.Products.Mapper;
using DashMart.Application.Products.Queries.Interface;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Products
{
    public sealed class ProductReadRepository : IProductReadRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductViewDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.PublicId == id);

            if (product == null) return null;

            return new ProductViewDto()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Grams = product.Weight,
                HowToUseNote = product.HowToUseNote,
                SKU = product.SKU
            };
        }


        public async Task<IReadOnlyList<ProductListDTO>> ListAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default)
         => ProductMapper.ToProductListDTO(await _context.Products
             .AsNoTracking()
             .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
             .ToListAsync(cancellationToken));

        public async Task<IReadOnlyList<ProductListDTO>> ListByCategoryIdAsync(Guid Id, int pageSize, int pageNumber, CancellationToken cancellationToken = default)
        {
            var category = await _context.Categories.AsNoTracking().SingleAsync(x => x.PublicId == Id, cancellationToken);

            return ProductMapper.ToProductListDTO(await _context.Products
            .AsNoTracking()
            .Where(x => x.ProductCategories.Any(x => x.CategoryId == category.Id))
            .Skip((pageNumber - 1 )* pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken));

        }
    }
}
