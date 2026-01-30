using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Products.Commands
{
    public sealed record UpdateProductSKUCommand
    (
        Guid PublicId,
        string SKU
        ) : IRequest<Result<string>>;

    public sealed class UpdateProductSKUCommandValidator : AbstractValidator<UpdateProductSKUCommand>
    {
        public UpdateProductSKUCommandValidator()
        {
            RuleFor(x=> x.SKU).NotNull().NotEmpty().WithMessage("SKU Cannot be null or empty")
                .Length(15).WithMessage("SKU length must be 15 length");
        }
    }

    internal sealed class UpdateProductSKUCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductSKUCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateProductSKUCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateProducts))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var product = await productRepo.GetByPublicIdAsync(request.PublicId, cancellationToken);

            if (product == null) return Result<string>.Failure($"Product with public Id {request.PublicId} not found", StatusCodeEnum.NotFound);

            var existingSKU = await productRepo.IsExistAsync(request.SKU, cancellationToken);

            if (existingSKU)
                return Result<string>.Failure("New SKU is exists", StatusCodeEnum.Conflict);

            product.UpdateSKU(SKU.Create(request.SKU));

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Product SKU Updated successfully");


        }
    }


}
