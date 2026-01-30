
using DashMart.Application.Products.DTOs;
using DashMart.Application.Products.Queries;
using DashMart.Application.Products.Queries.Interface;
using NSubstitute;
using DashMart.Application.Abstraction;

namespace ApplicationTest.Products.Query
{
    public class GetAllProductsByCategoryIdQueryHandlerTests
    {
        private readonly GetAllProductsByCategoryIdQueryHandler _handler;
        private readonly IProductReadRepository _productReadRepositoryMock;

        public GetAllProductsByCategoryIdQueryHandlerTests()
        {
            _productReadRepositoryMock = Substitute.For<IProductReadRepository>();
            _handler = new GetAllProductsByCategoryIdQueryHandler(_productReadRepositoryMock);
        }

        [Fact]
        public async Task Should_Returns_List_Of_ProductListDto_Successfully()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var request = new GetAllProductsByCategoryIdQuery(categoryId, ApplicationSettings.DefaultPageSize, ApplicationSettings.DefaultPageNumber);
            var expectedProductListDto = new List<ProductListDTO>() { new ProductListDTO() {Name = "Test", Grams = 200, Price = 1499 } };

            _productReadRepositoryMock.ListByCategoryIdAsync(
                categoryId, ApplicationSettings.DefaultPageSize, ApplicationSettings.DefaultPageNumber, CancellationToken.None
                ).Returns(expectedProductListDto);

            // Act
            var result = await _handler.Handle(request,CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("Test", result?.Value?.First().Name);

            // Verify
            await _productReadRepositoryMock.Received(1).ListByCategoryIdAsync(
                categoryId, ApplicationSettings.DefaultPageSize, ApplicationSettings.DefaultPageNumber, CancellationToken.None);
        }
        

    }
}
