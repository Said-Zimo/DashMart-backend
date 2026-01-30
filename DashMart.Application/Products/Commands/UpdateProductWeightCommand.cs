using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Products.Commands
{
    public sealed record UpdateProductWeightCommand(
        Guid PublicId,
        int Weight
        ) : IRequest<Result<string>>;


    public sealed class UpdateProductWeightCommandValidator : AbstractValidator<UpdateProductWeightCommand>
    {
        public UpdateProductWeightCommandValidator()
        {
            RuleFor(x => x.Weight).GreaterThan(0).WithMessage("Weight cannot be equal or less than 0");
        }
    }

    internal sealed class UpdateProductWeightCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductWeightCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateProductWeightCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdateProducts))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var product = await productRepo.GetByPublicIdAsync(request.PublicId, cancellationToken);

            if (product == null) return Result<string>.Failure($"Product with public Id {request.PublicId} not found", StatusCodeEnum.NotFound);

            var weight = Weight.Create(request.Weight);

            product.UpdateWeight(weight);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Product weight updated successfully");
        }
    }


}
