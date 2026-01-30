

using DashMart.Application.CurrentUserService;
using DashMart.Application.Products.Commands;
using DashMart.Domain.People.Users;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ApplicationTest.Products.Commands
{
    public class ReduceStockQuantityCommandTests
    {
        private readonly ReduceStockQuantityCommandHandler _handler;
        private readonly IProductRepository _productRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public ReduceStockQuantityCommandTests()
        {
            _productRepo = Substitute.For<IProductRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _currentUserService = Substitute.For<ICurrentUserService>();
            _handler = new ReduceStockQuantityCommandHandler(_currentUserService, _productRepo, _unitOfWork);
        }


        [Fact]
        public async Task Should_Returns_Failure_When_Current_User_Does_Not_Have_Permission()
        {
            Guid ProductId = Guid.NewGuid();
            var stockQuantity = 10;
            var command = new ReduceStockQuantityCommand(ProductId, stockQuantity);

            _currentUserService.HasPermission(UserPermissionsEnum.UpdateProducts).Returns(false);

            var result = await _handler.Handle(command, CancellationToken.None);


            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Access Denied", result.ErrorMessages?.First());


            _unitOfWork.DidNotReceiveWithAnyArgs();
            _productRepo.DidNotReceiveWithAnyArgs();
        }


        [Fact]
        public async Task Should_Returns_Failure_When_Product_Not_Found()
        {
            // Arrange
            Guid ProductId = Guid.NewGuid();
            var stockQuantity = 10;
            var command = new ReduceStockQuantityCommand(ProductId, stockQuantity);

            _currentUserService.HasPermission(UserPermissionsEnum.UpdateProducts).Returns(true);
            _productRepo.GetByPublicIdAsync(ProductId, CancellationToken.None).ReturnsNull();

            var result = await _handler.Handle(command , CancellationToken.None);


            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Product cannot found", result.ErrorMessages?.First());


            _unitOfWork.DidNotReceiveWithAnyArgs();


        }



    }
}
