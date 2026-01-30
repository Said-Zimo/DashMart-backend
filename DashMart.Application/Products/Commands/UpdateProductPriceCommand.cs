using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Products.Commands
{
    public sealed record UpdateProductPriceCommand(
        Guid ProductId,
        decimal Price
        ): IRequest<Result<string>>;

    public sealed class UpdateProductPriceCommandValidator : AbstractValidator<UpdateProductPriceCommand>
    {
        public UpdateProductPriceCommandValidator()
        {
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price cannot be zero or negative");
        }
    }

    internal sealed class UpdateProductPriceCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductPriceCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateProductPriceCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateProducts))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var product = await productRepo.GetByPublicIdAsync(request.ProductId, cancellationToken);

            if (product == null) return Result<string>.Failure($"Product with public Id {request.ProductId} not found", StatusCodeEnum.NotFound);

            var price = Price.Create(request.Price);

            product.UpdatePrice(price);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Product price updated successfully");

        }
    }

}
