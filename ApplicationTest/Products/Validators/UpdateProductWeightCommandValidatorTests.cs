

using DashMart.Application.Products.Commands;
using FluentValidation.TestHelper;

namespace ApplicationTest.Products.Validators
{
    public class UpdateProductWeightCommandValidatorTests
    {
        private readonly UpdateProductWeightCommandValidator _validator;

        public UpdateProductWeightCommandValidatorTests()
        {
            _validator = new UpdateProductWeightCommandValidator();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public void Should_Have_Error_When_Weight_Is_Invalid(int invalidWeight)
        {
            var command = new UpdateProductWeightCommand(new Guid(), invalidWeight );
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Weight)
                  .WithErrorMessage("Weight cannot be equal or less than 0");
        }

        [Fact]
        public void Should_Pass_When_Weight_Is_Positive()
        {
            var command = new UpdateProductWeightCommand(new Guid(), 3);
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Weight);
        }
    }

}
