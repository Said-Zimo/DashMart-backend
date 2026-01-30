

using DashMart.Application.Products.Commands;
using FluentValidation.TestHelper;

namespace ApplicationTest.Products.Validators
{
    public class UpdateProductSKUCommandValidatorTests
    {
        private readonly UpdateProductSKUCommandValidator _validator;

        public UpdateProductSKUCommandValidatorTests()
        {
            _validator = new UpdateProductSKUCommandValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123-5678-901--9234")]
        [InlineData("1234567890123456")]
        public void Should_Have_Error_When_SKU_Is_Null_Empty_Or_Less_Than_Valid_Length(string? invalidSku)
        {
            var command = new UpdateProductSKUCommand(new Guid(), invalidSku! );
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.SKU);
        }


        [Fact]
        public void Should_Pass_When_SKU_Length_Is_Exactly_15()
        {
            var validSku = new string('A', 15);

            var command = new UpdateProductSKUCommand(new Guid(), validSku );
            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }

}
