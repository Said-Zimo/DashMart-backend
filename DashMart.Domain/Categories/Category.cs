

using DashMart.Domain.Abstraction;
using DashMart.Domain.Validations;

namespace DashMart.Domain.Categories
{
    public sealed class Category : Aggregate
    {
        public string CategoryName { get; private set; } = default!;

        private readonly List<ProductCategory> _productCategories = new();
        public IReadOnlyCollection<ProductCategory> ProductCategories => _productCategories;

        private Category() { }

        private Category (string name)
        {
            DomainValidation.EnsureValidString(name, 50, "Category Name");
            CategoryName = name;
        }

        public static Category Create(string name)
        {
            return new Category(name);
        }

        public void UpdateName(string newName)
        {
            DomainValidation.EnsureValidString(newName, 50, "Category Name");

            CategoryName = newName;
        }

    }
}
