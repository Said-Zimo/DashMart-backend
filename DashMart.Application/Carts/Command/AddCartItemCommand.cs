using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Users;
using DashMart.Domain.Carts;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Carts.Command
{
    public sealed record AddCartItemCommand
    (
        Guid CartId,
        Guid ProductId,
        int Quantity
        ) : IRequest<Result<string>>;

    public sealed class AddCartItemCommandValidator : AbstractValidator<AddCartItemCommand>
    {
        public AddCartItemCommandValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0).LessThanOrEqualTo(32767).WithMessage("Quantity out of valid range");
        }
    }


    internal sealed class AddCartItemCommandHandler
        (ICurrentUserService currentUser, ICartRepository cartRepo, IProductRepository productRepo, IUnitOfWork unitOfWork)
        : IRequestHandler<AddCartItemCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {

            var cart = await cartRepo.GetByPublicIdAsync(request.CartId, cancellationToken);

            if (cart == null)
                return Result<string>.Failure("Cart not found", StatusCodeEnum.NotFound);

            var isOwner = cart.Customer.PublicId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.AccessCart);

            if (!isOwner && !isUser)
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var product = await productRepo.GetByPublicIdAsync(request.ProductId, cancellationToken);

            if (product == null)
                return Result<string>.Failure("Product not found", StatusCodeEnum.NotFound);

            cart.AddCartItem(product.Id, (short)request.Quantity, product.Price);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Cart item added successfully");


        }
    }


}
