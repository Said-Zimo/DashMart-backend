

using DashMart.Domain.Abstraction;
using DashMart.Domain.Categories;
using DashMart.Domain.Products;

namespace DomainTest
{
    public static class ProductTestFactory
    {
        public static Product CreateForTesting()
        {
            return Product.Create("test", "test", "test", Weight.Create(100), Price.Create(100), SKU.Create("1234-2634-8547-5487"));
        }

    }

    public class ProductTests
    {
        

        // Weight Testing
        [Fact]
        public void Create_NegativeAmount_ThrowsDomainException()
        {
            // Arrange
            var grams = 0;

            // Act & Assert
            Assert.Throws<DomainException>(() => { Weight.Create(grams); });
        }

        [Fact]
        public void Create_PositiveAmount_ReturnsNewWeightObject()
        {
            // Arrange
            int grams = 5;

            // Act 
            var weight = Weight.Create(grams);

            // Assert

            Assert.NotNull(weight);
            Assert.Equal(grams, weight.Value);

        }


        // Price Testing
        [Fact]
        public void Create_NegativePrice_ThrowsDomainException()
        {
            // Arrange
            decimal amount = 0;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                Price.Create(amount);
            });
        }

        [Fact]
        public void Create_PositivePrice_ReturnsNewPriceObject()
        {
            // Arrange
            decimal amount = 5;

            // Act 
            var price = Price.Create(amount);

            // Assert

            Assert.NotNull(price);
            Assert.Equal(amount, price.Value);
        }


        // SKU Testing
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("87-8")]
        public void Create_Invalid_ThrowsDomainException(string? sku)
        {
            // Act & Assert

            Assert.Throws<DomainException>(() =>
            {
                SKU.Create(sku!);
            });

        }

        [Theory]
        [InlineData("1234-1234-8978-7897")]
        [InlineData("A1B2-C3D4")]
        [InlineData("AB12-9F4C-77D")]
        [InlineData("123-AB9-X7Q")]
        public void Create_ValidSku_ReturnsNewSkuObject(string validSkuString)
        {

            // Act 
            var sku = SKU.Create(validSkuString);

            // Assert

            Assert.NotNull(sku);
            Assert.Equal(validSkuString, sku.Value);

        }



        // Product Aggregate Testing
        [Fact]
        public void Create_CreatingNewProduct_ReturnsNewProductObject()
        {
            //Arrange
            string name = "Mobile";
            string? description = null;
            string? howToUse = null;
            Weight weight = Weight.Create(100);
            Price price = Price.Create(100);
            SKU sku = SKU.Create("3652-5457-8569-7412");


            // Act 
            var product = Product.Create(name, description, howToUse, weight, price, sku);


            // Assert

            Assert.NotNull(product);

            Assert.Equal(name, product.Name);
            Assert.Null(product.Description);
            Assert.Null(product.HowToUseNote);
            Assert.NotNull(product.Weight);
            Assert.NotNull(product.Price);
            Assert.NotNull(product.SKU);
            Assert.Equal(weight,  product.Weight);
            Assert.Equal(price,  product.Price);
            Assert.Equal(sku,  product.SKU);

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_WrongProductName_ThrowException(string? name)
        {

            // Arrange
            var expectedWeight = Weight.Create(100);
            var expectedPrice = Price.Create(100);
            var expectedSKU = SKU.Create("1235-6587-8584-8478");

            // Act & Assert

            Assert.Throws<DomainException>(() =>
            {
                Product.Create(name!, null, null, expectedWeight, expectedPrice, expectedSKU);
            });

        }

        

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public void ReduceStockQuantity_ZeroOrNegative_ThrowException(int amount)
        {
            var product = ProductTestFactory.CreateForTesting();

            int currentQuantity = product.StockQuantity;

            Assert.Throws<DomainException>(()=>  product.ReduceStockQuantity(amount));

        }

        [Fact]
        public void ReduceStockQuantity_AmountEqualCurrentStockQuantity()
        {
            var product = ProductTestFactory.CreateForTesting();

            product.InsertStockQuantity(1);

            int currentQuantity = product.StockQuantity;
            var amount = currentQuantity;

            product.ReduceStockQuantity(amount);

            Assert.False(product.IsActive);
        }


        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        public void UpdateDetails_WrongName_ThrowException(string? name)
        {
            Product product = ProductTestFactory.CreateForTesting();

            Assert.Throws<DomainException>(()=> product.UpdateDetails(name!, null, null));
        }


        [Fact]
        public void UpdateDetails_WrongDescription_ThrowException()
        {
            var description = new string('a', 600);
            Product product = ProductTestFactory.CreateForTesting();

            Assert.Throws<DomainException>(() => product.UpdateDetails("test", description, null));
        }

        [Fact]
        public void UpdateDetails_WrongHowToUse_ThrowException()
        {
            var howToUse = new string('a', 300);
            Product product = ProductTestFactory.CreateForTesting();

            Assert.Throws<DomainException>(() => product.UpdateDetails("test", null, howToUse));
        }



        [Fact]
        public void InsertStockQuantity_AmountPositive_UpdatedQuantity()
        {
            // Arrange
            var product = ProductTestFactory.CreateForTesting();


            // Act & Assert
            product.InsertStockQuantity(1);

            Assert.Equal(1, product.StockQuantity);
        }

        [Fact]
        public void InsertStockQuantity_AmountNegative_ThrowDomainException()
        {
            // Arrange
            var product = ProductTestFactory.CreateForTesting();



            // Act & Assert
            var exception = Assert.Throws<DomainException>(() =>
            {
                product.InsertStockQuantity(-1);
            });

            Assert.Equal("Amount cannot be negative", exception.Message);
        }


        [Fact]
        public void AddToCategory_AddingExistingCategory_ThrowDomainException()
        {
            var product = ProductTestFactory.CreateForTesting();
            product.Id = 10;

            var category = Category.Create("Games");
            category.Id = 10;
            product.AddToCategory(category.Id);

            Assert.Throws<DomainException>(() =>
            {
                product.AddToCategory(category.Id);
            });
        }


        [Fact]
        public void AddToCategory_AddingDifferentCategory_AddedSuccessfully()
        {
            //Arrange
            var product = ProductTestFactory.CreateForTesting();
            product.Id = 10;

            var category1 = Category.Create("Games");
            category1.Id = 10;
            product.AddToCategory(category1.Id);

            var category2 = Category.Create("PC");
            category2.Id = 11;
            product.AddToCategory(category2.Id);

            Assert.True(product.HasCategory(category1.Id));
            Assert.True(product.HasCategory(category2.Id));


        }


        [Fact]
        public void DeleteFromCategory_NotFoundCategoryToDelete_ThrowsDomainException()
        {
            var product = ProductTestFactory.CreateForTesting();

            Assert.Throws<DomainException>(() => 
            {
                product.DeleteFromCategory(200);
            });
        }

        [Fact]
        public void DeleteFromCategory_ExistingCategory_DeletedSuccessfully()
        {
            var product = ProductTestFactory.CreateForTesting();
            product.Id = 10;
            int Id = 10;
            var category = Category.Create("Games");
            category.Id = Id;

            product.AddToCategory(category.Id);
            Assert.True(product.HasCategory(Id));


            product.DeleteFromCategory(Id);
            Assert.False(product.HasCategory(Id));

        }


        [Fact]
        public void AddImages_AddingCollectionOfImagesMoreThanAllowedRangeImages_ThrowsDomainException()
        {
            var products = ProductTestFactory.CreateForTesting();

            string[] images = 
            {
                "Test1",
                "Test2",
                "Test3"
            };

            Assert.Throws<DomainException>(() =>
            {
                products.AddImages(images);
            });
        }




    }
}
