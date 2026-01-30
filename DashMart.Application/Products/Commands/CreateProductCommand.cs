
using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Products;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Products.Commands
{
    public sealed record CreateProductCommand
      (
        string Name,
        string? Description,
        string? HowToUseNote,
        int Grams,
        decimal Price,
        string SKU,
        int StockQuantity
        ) : IRequest<Result<string>>;


    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("Name cannot be null or empty");
            RuleFor(x => x.Name).MinimumLength(3).MaximumLength(50).WithMessage("Product Name range must be between 3 - 50 length");

            RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be less than 0");

            RuleFor(x => x.SKU).NotNull().NotEmpty().WithMessage("SKU Cannot be null or empty");

            RuleFor(x => x.Grams).GreaterThan(0).WithMessage("Weight must be greater then 0");

            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price cannot be less than 0");

        }
    }


    internal sealed class CreateProductCommandHandler
        (ICurrentUserService currentUser, IProductRepository productRepo, IUnitOfWork unitOfWork) 
        : IRequestHandler<CreateProductCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.CreateProduct))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            bool isExistingSku = !await productRepo.IsExistAsync(request.SKU, cancellationToken);

            if (!isExistingSku)
                return Result<string>.Failure("Product with this SKU already exists", StatusCodeEnum.Conflict);
            

            var product = Product.Create(request.Name, request.Description, request.HowToUseNote,
                 Weight.Create(request.Grams),Price.Create(request.Price),SKU.Create( request.SKU));

            productRepo.Add(product);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Product created successfully");
        }
    }

}
