

using DashMart.Application.Products.Commands;
using FluentValidation.TestHelper;

namespace ApplicationTest.Products.Validators
{
    public class CreateProductCommandValidatorTests
    {
        private readonly CreateProductCommandValidator _validator;
        public CreateProductCommandValidatorTests()
        {
            _validator = new CreateProductCommandValidator();
        }

        [Theory] 
        [InlineData(null)]       
        [InlineData("")]         
        [InlineData("ab")]       
        public void Should_Have_Error_When_Name_Is_Invalid(string? invalidName)
        {
            // Arrange
            var command = new CreateProductCommand(invalidName! , null, null, 2, 5, "2365-5258", 2 );

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Exceeds_50_Characters()
        {
            var longName = new string('a', 51);
            var command = new CreateProductCommand(longName, null, null, 2, 5,  "2365-5258", 2);
            
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }


        [Fact]
        public void Should_Not_Have_Error_When_Name_Is_Valid()
        {
            var command = new CreateProductCommand("Valid Product Name", null, null, 2, 5, "2365-5258", 2);
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        
        [Fact]
        public void Should_Have_Error_When_StockQuantity_Is_Negative()
        {
            var command = new CreateProductCommand("Valid Product Name", null, null, 2, 5, "2365-5258", -1);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.StockQuantity)
                  .WithErrorMessage("Stock quantity cannot be less than 0");
        }


        [Fact]
        public void Should_Not_Have_Error_When_StockQuantity_Is_Zero_Or_More()
        {
            var command = new CreateProductCommand("Valid Product Name", null, null, 2, 5,  "2365-5258", 0);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.StockQuantity);
        }

        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Have_Error_When_SKU_Is_Null_Or_Empty(string? invalidSku)
        {
            var command = new CreateProductCommand("Valid Product Name", null, null, 2, 5,invalidSku!, 2);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.SKU);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-5)] 
        public void Should_Have_Error_When_Grams_Is_Zero_Or_Less(int invalidGrams)
        {
            var command = new CreateProductCommand("Valid Product Name", null, null, invalidGrams, 5, "254-5854", 2);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Grams);
        }

        

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Should_Have_Error_When_Price_Is_Zero_Or_Less(decimal invalidPrice)
        {
            var command = new CreateProductCommand("Valid Product Name", null, null, 1, invalidPrice,  "254-5854", 2);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

       

        [Fact]
        public void Should_Pass_When_All_Data_Is_Correct()
        {
            var command = new CreateProductCommand("iPhone 15", null, null, 200, 999.99m, "APL-15-PRO", 10);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }

}
