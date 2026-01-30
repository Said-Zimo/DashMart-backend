

using DashMart.Domain.Products;
using DashMart.Application.Products.Commands;
using DashMart.Application.Products.DTOs;

namespace DashMart.Application.Products.Mapper
{
    public static class ProductMapper
    {
        public static IReadOnlyList<ProductListDTO> ToProductListDTO(IList<Product> products)
        {
            var list = new List<ProductListDTO>();

            foreach (var product in products)
            {
                list.Add(new ProductListDTO { Name = product.Name, Grams = product.Weight, Price = product.Price });
            }

            return list;

        }
         

    }
}
