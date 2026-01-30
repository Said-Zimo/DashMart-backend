

using DashMart.Application.Products.Commands;
using FluentValidation.TestHelper;

namespace ApplicationTest.Products.Validators
{
    public class UpdateProductPriceCommandValidatorTests
    {
        private readonly UpdateProductPriceCommandValidator _validator;

        public UpdateProductPriceCommandValidatorTests()
        {
            _validator = new UpdateProductPriceCommandValidator();
        }

        [Theory]
        [InlineData(0)]   
        [InlineData(-1)]  
        [InlineData(-100)]
        public void Should_Have_Error_When_Price_Is_Zero_Or_Less(decimal invalidPrice)
        {
            var command = new UpdateProductPriceCommand(new Guid(), invalidPrice );
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Price)
                  .WithErrorMessage("Price cannot be zero or negative");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Price_Is_Valid()
        {
            var command = new UpdateProductPriceCommand(new Guid(), 100);
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }
    }

}
