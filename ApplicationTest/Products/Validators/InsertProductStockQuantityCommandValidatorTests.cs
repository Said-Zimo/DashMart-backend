

using DashMart.Application.Products.Commands;
using FluentValidation.TestHelper;

namespace ApplicationTest.Products.Validators
{
    public class InsertProductStockQuantityCommandValidatorTests
    {
        private readonly InsertProductStockQuantityCommandValidator _validator;
        public InsertProductStockQuantityCommandValidatorTests()
        {
            _validator = new();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Should_Have_Error_When_Inserting_Not_Valid_Quantity(int quantity)
        {
            var command = new InsertProductStockQuantityCommand(new Guid(), quantity);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrors();
        }

        [Fact]
        public void Should_Not_Have_Error_When_Inserting_Valid_Quantity()
        {
            var command = new InsertProductStockQuantityCommand(new Guid(), 50);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }



    }
}
