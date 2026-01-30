
using DashMart.Domain.Products;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Products
{
    public class ProductRepository(ApplicationDbContext _context) : IProductRepository
    {

        public void Add(Product product) => _context.Products.Add(product);

        public void Delete(Product product)
        {
            product.IsDeleted = true;
            product.DeletedAt = DateTime.Now;
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Products.Include(x=> x.ProductCategories).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        
        public async Task<Product?> GetByPublicIdAsync(Guid Id, CancellationToken cancellationToken)
        => await _context.Products.Include(x => x.ProductCategories).FirstOrDefaultAsync(p => p.PublicId == Id, cancellationToken);

        public async Task<bool> IsExistAsync(string sku, CancellationToken cancellationToken)
        => await _context.Products.AnyAsync(x=> x.SKU.Value == sku, cancellationToken);

        
    }
}
