

using DashMart.Application.Products.Commands;
using FluentValidation.TestHelper;

namespace ApplicationTest.Products.Validators
{
    public class UpdateProductDetailsCommandValidatorTests
    {

        private readonly UpdateProductDetailsCommandValidator _validator;


        public UpdateProductDetailsCommandValidatorTests()
        {
            _validator = new();
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("ab")]
        public void Should_Have_Error_When_Name_Is_Invalid(string? invalidName)
        {
            // Arrange
            var command = new UpdateProductDetailsCommand(new Guid(), invalidName!, null, null);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Exceeds_50_Characters()
        {
            var longName = new string('a', 51);
            var command = new UpdateProductDetailsCommand(new Guid(), longName, null, null);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }


        [Fact]
        public void Should_Not_Have_Error_When_Valid_Data()
        {
            var command = new UpdateProductDetailsCommand(new Guid(), "ValidName", null, null);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }


    }
}
