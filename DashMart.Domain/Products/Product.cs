using DashMart.Domain.Abstraction;
using DashMart.Domain.Categories;
using DashMart.Domain.Validations;
using System.Text.RegularExpressions;


namespace DashMart.Domain.Products
{
    public sealed class Product : Aggregate
    {
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }
        public Weight Weight { get; private set; } = default!;
        public Price Price { get; private set; } = default!;
        public string? HowToUseNote { get; private set; }
        public SKU SKU { get; private set; } = default!;
        public int StockQuantity { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<ProductCategory> _productCategories = new();
        public IReadOnlyCollection<ProductCategory> ProductCategories => _productCategories;

        private readonly List<ProductImageGallery> _productImages = new();
        public IReadOnlyCollection<ProductImageGallery> ProductImages => _productImages;

        private Product() { }

        private Product(string name, string? description, string? howToUseNote, Weight grams, Price price, SKU sKU, bool isActive)
        {

            DomainValidation.EnsureValidString(name, 50, "Product Name");

            if (!string.IsNullOrWhiteSpace(description)) DomainValidation.EnsureValidString(description, 500, "Product Description");
            if (!string.IsNullOrWhiteSpace(howToUseNote)) DomainValidation.EnsureValidString(howToUseNote, 250, "Product how to use note");

            Name = name;
            Description = description;
            Weight = grams;
            Price = price;
            HowToUseNote = howToUseNote;
            SKU = sKU;
            IsActive = isActive;
        }

        public static Product Create(string name, string? description, string? howToUseNote, Weight grams, Price price, SKU sku)
        {

            return new Product (name, description,howToUseNote, grams, price,sku,true);
        }

        public void InsertStockQuantity(int amount)
        {
            if (amount < 0) throw new DomainException("Amount cannot be negative");

            StockQuantity += amount;
        }

        public void ReduceStockQuantity(int amount)
        {
            if (amount <= 0) throw new DomainException("Amount cannot be negative");

            if (amount > StockQuantity) throw new DomainException("Result of enucleation cannot be negative");

            if (amount == StockQuantity)
                IsActive = false;
            
            StockQuantity -= amount;
        }

        public void UpdateDetails(string name, string? description, string? howToUseNote)
        {
            UpdateName(name);
            UpdateDescription(description);
            UpdateHowToUseNote(howToUseNote);
        }

        public void AddToCategory(int categoryId)
        {
            if (_productCategories.Any(x => x.CategoryId == categoryId)) throw new DomainException("This category already has been added before");

            _productCategories.Add(ProductCategory.Create(this.Id, categoryId));
        }

        public bool HasCategory(int categoryId)
        {
           return _productCategories.Any(x => x.CategoryId == categoryId);
        }

        public void UpdateCategory(int currentCategoryId , int newCategoryId)
        {
            var category = _productCategories.FirstOrDefault(x=> x.CategoryId == currentCategoryId) 
                ?? throw new DomainException("Category not found") ;

            if (_productCategories.Any(x => x.CategoryId == newCategoryId))
                throw new DomainException("New category is already exists");

            _productCategories.Remove(category);

            _productCategories.Add(ProductCategory.Create(this.Id, newCategoryId));

        }

        public void DeleteFromCategory(int categoryId)
        {
            var category = _productCategories.FirstOrDefault(x=> x.CategoryId == categoryId) ?? throw new DomainException("Category not found");

            _productCategories.Remove(category);
        }

        private void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Product name cannot be Null");

            if (name.Length > 50) throw new DomainException("Product name length cannot be larger than 50");

            Name = name;
        }

        public void UpdateImage(Guid Id , string imagePath)
        {
            var image = _productImages.FirstOrDefault(x=> x.PublicId == Id) 
                ?? throw new DomainException("Image not found");

            image.Update(imagePath);
        }

        public void RemoveImageFromGallery(Guid imageId)
        {
            var image = _productImages.FirstOrDefault(x => x.PublicId == imageId)
                ?? throw new DomainException("Image not found");

            _productImages.Remove(image);
        }

        public void AddImage(string imagePath, bool isMain)
        {
            if (_productImages.Count >= 2) throw new DomainException("Cannot add more than 2 images");

            var mainsCount = _productImages.Where(x=> x.IsMain).ToList();

            if(isMain &&  mainsCount.Count < 1) isMain = false;

            _productImages.Add(ProductImageGallery.Create(imagePath, isMain));
        }

        public void AddImages(ICollection<string> imagePaths)
        {
            if (imagePaths.Count + _productImages.Count > 2)
                throw new DomainException("Cannot add more than 2 images");

            bool isMain = true;
            foreach (var imagePath in imagePaths)
            {
                _productImages.Add(ProductImageGallery.Create(imagePath, isMain));
                isMain = false;
            }
        }

        public void UpdateSKU(SKU sku)
        {
            if (sku == null)
                throw new DomainException("SKU cannot be null");

            SKU = sku;
        }

        private void UpdateDescription(string? description)
        {
            if (description?.Length > 500) throw new DomainException("Description cannot be longer than 500 characters");

            Description = description;
        }

        private void UpdateHowToUseNote(string? note)
        {
            if (note?.Length > 250) throw new DomainException("Note cannot be longer than 250 characters");

            HowToUseNote = note;
        }

        public void Deactivate() => IsActive = false;

        public void Activate() => IsActive = true;

        public void UpdatePrice(Price newPrice)
        {
            if (newPrice == null)
                throw new DomainException("Price cannot be null");

            Price =  newPrice;
        }

        public void UpdateWeight(Weight newWeight)
        {
            if (newWeight == null)
                throw new DomainException("Weight cannot be null");

            Weight = newWeight;
        }

    }

    public sealed record SKU
    {
        public string Value { get; } = null!;
        private SKU() { }

        private SKU(string sku)
        {
            if (!IsSKUFormat(sku)) throw new DomainException("SKU dos not match the true format");

            Value = sku;
        }

        public static SKU Create(string sku) => new SKU(sku);

        public bool IsSKUFormat(string sku)
        {
            if (string.IsNullOrEmpty(sku)) return false;
                
            var pattern = @"^([A-Za-z0-9]{3,4})(-([A-Za-z0-9]{3,4})){1,3}$";
            return Regex.IsMatch(sku, pattern);
        }


        public static implicit operator string(SKU sku) => sku.Value;
    }

    public sealed record Price
    {
        public decimal Value { get;}

        private Price() { }
        private Price(decimal value)
        {
            if (value <= 0)
                throw new DomainException("Weight cannot be equal 0 or less than 0");

            Value = value;
        }

        public static Price Create (decimal price) => new Price(price);

        public int WithoutBreaks() => (int)Value;
        public override string ToString() => $"{Value}TL";

        public static implicit operator decimal(Price price) => price.Value;
    }

    public sealed record Weight
    {
        public int Value { get; }

        private Weight() { }

        private Weight(int weight)
        {
            if (weight <= 0)
                throw new DomainException("Weight cannot be negative or equal 0");

            Value = weight;
        }

        public static Weight Create (int value) => new Weight(value);

        public decimal ToKilograms() => Value / 1000m;

        public override string ToString() => Value < 1000 ? $"{Value} g" : $"{ToKilograms()} kg";

        public static implicit operator int(Weight weight) => weight.Value;
    }


}
