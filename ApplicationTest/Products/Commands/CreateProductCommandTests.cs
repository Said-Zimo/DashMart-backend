

using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using DashMart.Application.Products.Commands;
using NSubstitute;
using DashMart.Application.CurrentUserService;
using DashMart.Domain.People.Users;

namespace ApplicationTest.Products.Commands
{
    public class CreateProductCommandTests
    {
        private readonly CreateProductCommandHandler _Handler;
        private readonly IProductRepository _productRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateProductCommandTests()
        {
            _productRepo = Substitute.For<IProductRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _currentUserService = Substitute.For<ICurrentUserService>();

            _Handler = new CreateProductCommandHandler(_currentUserService, _productRepo, _unitOfWork);
        }


        [Fact]
        public async Task Should_Return_Failure_When_Current_User_Does_Not_Have_Permission()
        {
            var command = new CreateProductCommand("15 pro", null, null, 20, 20, "sda-jyve-fsuw-58ew", 2);

            _currentUserService.HasPermission(UserPermissionsEnum.CreateProduct).Returns(false);

            var result = await _Handler.Handle(command, CancellationToken.None);


            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Access Denied", result.ErrorMessages?.First());

            _productRepo.ReceivedWithAnyArgs();
            _unitOfWork.DidNotReceiveWithAnyArgs();

        }


        [Fact]
        public async Task Should_Returns_Failure_When_Product_sku_Is_Exist()
        {
            string sku = "655-487f-sef8-878";
            var command = new CreateProductCommand("test", null, null, 20, 30, sku, 40);

            _currentUserService.HasPermission(UserPermissionsEnum.CreateProduct).Returns(true);
            _productRepo.IsExistAsync(sku, CancellationToken.None).Returns(true);


            var result = await _Handler.Handle(command, CancellationToken.None);


            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Product with this SKU already exists", result.ErrorMessages?.First());

            _productRepo.ReceivedWithAnyArgs();
            _unitOfWork.DidNotReceiveWithAnyArgs();
        }



        [Fact]
        public async Task Should_Create_Product_Success_When_Every_Conditions_True()
        {
            string sku = "5ew-n5ue-s8u-123";
            var command = new CreateProductCommand("test", null, null, 20, 30, sku, 40);

            _currentUserService.HasPermission(UserPermissionsEnum.CreateProduct).Returns(true);
            _productRepo.IsExistAsync(sku, CancellationToken.None).Returns(false);


            var result = await _Handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("Product created successfully", result.Value);

            await _productRepo.Received(1).IsExistAsync(sku, CancellationToken.None);
            _productRepo.Received(1).Add(Arg.Is<Product>(p=> p.Name == command.Name && p.SKU == command.SKU));
            await _unitOfWork.Received(1).SaveChangeAsync(Arg.Any<CancellationToken>());

        }


    }
}
