

using DashMart.Application.Products.Queries;
using DashMart.Application.Products.Queries.Interface;
using NSubstitute;
using DashMart.Application.Products.DTOs;

namespace ApplicationTest.Products.Query
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly GetProductByIdQueryHandler _handler;
        private readonly IProductReadRepository _productReadRepository;

        public GetProductByIdQueryHandlerTests()
        {
            _productReadRepository = Substitute.For<IProductReadRepository>();
            _handler = new GetProductByIdQueryHandler( _productReadRepository);
        }



        [Fact]
        public async Task Should_Returns_ProductViewDTO_When_Product_Is_Founded()
        {
            var productId = Guid.NewGuid();
            var request = new GetProductByIdQuery(productId);

            var expectedProductViewDto = new ProductViewDto() { Name = "IPhone 15 pro" };

            _productReadRepository.GetByIdAsync(productId, CancellationToken.None).Returns(expectedProductViewDto);


            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(expectedProductViewDto.Name, result?.Value?.Name);

            await _productReadRepository.Received().GetByIdAsync(productId, CancellationToken.None);

        }

    }
}
