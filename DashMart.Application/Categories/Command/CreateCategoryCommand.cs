using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Users;
using DashMart.Domain.Categories;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Categories.Command
{
    public sealed record CreateCategoryCommand
    (
        string Name
        ) : IRequest<Result<string>>;

    public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Category name cannot be null or empty")
                .MaximumLength(50).WithMessage("Category name length cannot be greater than 50 character");
        }
    }

    internal sealed class CreateCategoryCommandHandler
        (ICurrentUserService currentUser, ICategoryRepository categoryRepo, IUnitOfWork unitOfWork)
        : IRequestHandler<CreateCategoryCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.AccessCategory))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);


            if (await categoryRepo.IsExistAsync(request.Name, cancellationToken))
                return Result<string>.Failure($"Category with name {request.Name} is already exists", StatusCodeEnum.Conflict);


            var category = Category.Create(request.Name);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Category created successfully");
                    

        }
    }

}
