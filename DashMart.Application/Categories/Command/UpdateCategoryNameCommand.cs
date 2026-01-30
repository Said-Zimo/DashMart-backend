using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Categories;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Categories.Command
{
    public sealed record UpdateCategoryNameCommand
    (
        Guid CategoryId,
        string Name
        ) : IRequest<Result<string>>;


    public sealed class UpdateCategoryNameCommandValidator : AbstractValidator<UpdateCategoryNameCommand>
    {
        public UpdateCategoryNameCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Category name cannot be null or empty")
                .MaximumLength(50).WithMessage("Category name length cannot be greater than 50 character");
        }
    }

    internal sealed class UpdateCategoryNameCommandHandler
        (ICurrentUserService currentUser, ICategoryRepository categoryRepo, IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateCategoryNameCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateCategoryNameCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.AccessCategory))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var category = await categoryRepo.GetByPublicIdAsync(request.CategoryId , cancellationToken);

            if (category == null)
                return Result<string>.Failure("Category not found", StatusCodeEnum.NotFound);

            if(await categoryRepo.IsExistAsync(request.Name))
                return Result<string>.Failure($"Category with name {request.Name} is already exists", StatusCodeEnum.Conflict);

            category.UpdateName(request.Name);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Category name updated successfully");
        }
    }

} 
