using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Products.Commands
{
    public sealed record UpdateProductDetailsCommand(
        Guid ProductId,
        string Name,
        string? Description,
        string? HowToUseNote
        ) : IRequest<Result<string>>;


    public sealed class UpdateProductDetailsCommandValidator : AbstractValidator<UpdateProductDetailsCommand>
    {
        public UpdateProductDetailsCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Product name cannot be null or empty")
                .MinimumLength(3).MaximumLength(50).WithMessage("Product name range must be between 3 - 50 length");
        }
    }

    internal sealed class UpdateProductDetailsCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductDetailsCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateProductDetailsCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateProducts))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var product = await productRepo.GetByPublicIdAsync(request.ProductId, cancellationToken);

            if (product == null) return Result<string>.Failure($"Product with public Id {request.ProductId} not found", StatusCodeEnum.NotFound);

            product.UpdateDetails(request.Name, request.Description, request.HowToUseNote);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Product details updated successfully");
        }
    }

}
