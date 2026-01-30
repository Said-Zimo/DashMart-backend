using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Products.Commands.ProductImageCommands
{
    public sealed record UpdateProductImageCommand(
        Guid ProductPublicId,
        Guid ImagePublicId,
        string ImagePath ) : IRequest<Result<string>>;


    public sealed class UpdateProductImageCommandValidator : AbstractValidator<UpdateProductImageCommand>
    {
        public UpdateProductImageCommandValidator()
        {
            RuleFor(x=> x.ImagePath).NotNull().NotEmpty().WithMessage("Image path cannot be null or empty");
        }
    }

    internal sealed class UpdateProductImageCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductImageCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateProductImageCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateProducts))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var product = await productRepo.GetByPublicIdAsync(request.ProductPublicId, cancellationToken);

            if (product == null) return Result<string>.Failure($"Product with public Id {request.ProductPublicId} not found", StatusCodeEnum.NotFound);

            product.UpdateImage(request.ImagePublicId, request.ImagePath);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Product image updated successfully");

        }
    }

}
