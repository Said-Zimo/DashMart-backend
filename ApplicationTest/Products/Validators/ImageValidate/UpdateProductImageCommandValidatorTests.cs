using DashMart.Application.Products.Commands.ProductImageCommands;
using FluentValidation.TestHelper;

namespace ApplicationTest.Products.Validators.ImageValidate
{
    public class UpdateProductImageCommandValidatorTests
    {
        private readonly UpdateProductImageCommandValidator _validator;

        public UpdateProductImageCommandValidatorTests()
        {
            _validator = new UpdateProductImageCommandValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Path_Is_Invalid(string? invalidPath)
        {
            var command = new UpdateProductImageCommand (new Guid(), new Guid(), ImagePath : invalidPath! );

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ImagePath)
                  .WithErrorMessage("Image path cannot be null or empty");
        }

        [Fact]
        public void Should_Pass_When_Path_Is_Valid()
        {
            var command = new UpdateProductImageCommand(new Guid(), new Guid(), ImagePath: "products/img123.png" );

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }

}
