

using DashMart.Application.Products.Commands;
using FluentValidation.TestHelper;

namespace ApplicationTest.Products.Validators
{
    public class ReduceStockQuantityCommandValidatorTests
    {

        private readonly ReduceStockQuantityCommandValidator _validator;

        public ReduceStockQuantityCommandValidatorTests()
        {
            _validator = new();
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Should_Have_Error_When_Invalid_Quantity(int quantity)
        {
            var command = new ReduceStockQuantityCommand(new Guid(), quantity);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrors();
        }


        [Fact]
        public void Should_Not_Have_Error_When_Inserting_Valid_Quantity()
        {
            var command = new ReduceStockQuantityCommand(new Guid(), 50);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }




    }
}
