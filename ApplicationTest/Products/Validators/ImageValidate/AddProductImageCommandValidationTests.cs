using DashMart.Application.Products.Commands.ProductImageCommands;
using FluentValidation.TestHelper;

namespace ApplicationTest.Products.Validators.ImageValidate
{
    public class AddProductImageCommandValidationTests
    {
        private readonly AddProductImageCommandValidation _validator;

        public AddProductImageCommandValidationTests()
        {
            _validator = new AddProductImageCommandValidation();
        }


        [Fact]
        public void Should_Have_Error_When_ImagePaths_List_Is_Null()
        {
            var command = new AddProductImagesCommand (new Guid(), null! );
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ImagePaths)
                  .WithErrorMessage("Image list cannot be null");
        }

        [Fact]
        public void Should_Have_Error_When_ImagePaths_List_Is_Empty()
        {
            var command = new AddProductImagesCommand(new Guid(), new List<string>() );
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ImagePaths)
                  .WithErrorMessage("You must provide at least one image path");
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")] 
        public void Should_Have_Error_When_An_Item_In_List_Is_Invalid(string? invalidPath)
        {
            var command = new AddProductImagesCommand(new Guid(),
                ImagePaths: new List<string> { "valid.jpg", invalidPath! });
            

            var result = _validator.TestValidate(command);


            result.ShouldHaveValidationErrorFor(x => x.ImagePaths)
                  .WithErrorMessage("One of the images is null");
        }

        [Fact]
        public void Should_Pass_When_List_And_Items_Are_Valid()
        {
            var command = new AddProductImagesCommand(new Guid(),

                ImagePaths: new List<string> { "image1.png", "image2.jpg" });
            

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }

}
