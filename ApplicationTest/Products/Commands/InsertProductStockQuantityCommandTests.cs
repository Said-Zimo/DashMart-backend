

using DashMart.Application.CurrentUserService;
using DashMart.Application.Products.Commands;
using DashMart.Domain.People.Users;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ApplicationTest.Products.Commands
{
    public class InsertProductStockQuantityCommandTests
    {

        private readonly InsertProductStockQuantityCommandHandler _handler ;
        private readonly IProductRepository _productRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public InsertProductStockQuantityCommandTests()
        {
            _productRepo = Substitute.For<IProductRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _currentUserService = Substitute.For<ICurrentUserService>();
            _handler = new InsertProductStockQuantityCommandHandler(_currentUserService, _productRepo , _unitOfWork);
        }

        [Fact]
        public async Task Should_Returns_Failure_When_Current_User_Does_Not_Have_Permission()
        {
            Guid productId = Guid.NewGuid();
            int quantity = 1;
            var command = new InsertProductStockQuantityCommand(productId, quantity);

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
            Guid productId = Guid.NewGuid();
            int quantity = 1;
            var command = new InsertProductStockQuantityCommand(productId, quantity);

            _currentUserService.HasPermission(UserPermissionsEnum.UpdateProducts).Returns(true);
            _productRepo.GetByPublicIdAsync(productId, CancellationToken.None).ReturnsNull();


            var result = await _handler.Handle(command, CancellationToken.None);


            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Product not found", result.ErrorMessages?.First());


            _unitOfWork.DidNotReceiveWithAnyArgs();
        }


        [Fact]
        public async Task Should_Insert_Product_Quantity_Stock_Success_Every_Conditions_Is_True()
        {

            Guid productId = Guid.NewGuid();
            int quantity = 1;
            var command = new InsertProductStockQuantityCommand(productId, quantity);

            var product = Product.Create("Test", null, null, Weight.Create(20), Price.Create(20), SKU.Create("564-5658-8587-pf7"));

            _currentUserService.HasPermission(UserPermissionsEnum.UpdateProducts).Returns(true);
            _productRepo.GetByPublicIdAsync(productId, CancellationToken.None).Returns(product);


            var result = await _handler.Handle(command, CancellationToken.None);


            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("Product stock updated successfully", result.Value);


            _unitOfWork.ReceivedWithAnyArgs();
        }


    }
}
