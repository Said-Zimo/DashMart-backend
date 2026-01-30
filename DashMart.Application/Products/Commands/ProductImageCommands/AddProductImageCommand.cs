using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Products.Commands.ProductImageCommands
{
    public sealed record AddProductImagesCommand
        (
        Guid ProductId,
        ICollection<string> ImagePaths
        ) : IRequest<Result<string>>;


    public sealed class AddProductImageCommandValidation : AbstractValidator<AddProductImagesCommand>
    {
        public AddProductImageCommandValidation()
        {
            RuleFor(x => x.ImagePaths)
            .NotNull().WithMessage("Image list cannot be null")
            .NotEmpty().WithMessage("You must provide at least one image path");

            RuleForEach(x=> x.ImagePaths).NotNull().NotEmpty().WithMessage("One of the images is null");
        }
    }

    internal sealed class AddProductImageCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, IUnitOfWork unitOfWork) : IRequestHandler<AddProductImagesCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(AddProductImagesCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateProducts))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var product = await productRepo.GetByPublicIdAsync(request.ProductId , cancellationToken);

            if (product == null) return Result<string>.Failure($"Product with public Id {request.ProductId} not found", StatusCodeEnum.NotFound);

            product.AddImages(request.ImagePaths);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Product image added successfully");
        }
    }

}
