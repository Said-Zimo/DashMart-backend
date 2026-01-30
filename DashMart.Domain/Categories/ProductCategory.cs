

using DashMart.Domain.Abstraction;
using DashMart.Domain.Products;

namespace DashMart.Domain.Categories
{
    public class ProductCategory
    {
        public int ProductId { get;private set; }

        public int CategoryId { get;private set; }

        public Product Product { get;private set; } = default!;

        public Category Category { get;private set; }= default!;

        private ProductCategory() { }

        private ProductCategory(int productId, int categoryId)
        {
            if(productId <= 0)  throw new DomainException("Product Id cannot be negative");
            if( categoryId <= 0)  throw new DomainException("Category Id cannot be negative");

            ProductId = productId;
            CategoryId = categoryId;
        }

        public static ProductCategory Create(int productId, int categoryId)
        {
            return new ProductCategory(productId, categoryId);
        }
    }
}
