using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Users;
using DashMart.Domain.Orders;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Orders.Command
{
    public sealed record SetOrderNoteCommand
    (
        Guid OrderId,
        string Note
        ) : IRequest<Result<string>>;


    public sealed class SetOrderNoteCommandValidator : AbstractValidator<SetOrderNoteCommand>
    {
        public SetOrderNoteCommandValidator()
        {
            RuleFor(x => x.Note).NotNull().NotEmpty().WithMessage("Note cannot be null or empty")
                .MaximumLength(50).WithMessage("Note length cannot be greater than 50 character");
        }
    }

    internal sealed class SetOrderNoteCommandHandler
        (ICurrentUserService currentUser, IOrderRepository orderRepo , IUnitOfWork unitOfWork)
        : IRequestHandler<SetOrderNoteCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(SetOrderNoteCommand request, CancellationToken cancellationToken)
        {

            var order = await orderRepo.GetByPublicIdAsync(request.OrderId, cancellationToken);

            if (order == null) return Result<string>.Failure("Order not found", StatusCodeEnum.NotFound);

            var isOwner = order.Customer.PublicId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.UpdateOrder);

            if (!isOwner && !isUser)
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            order.SetNote(request.Note);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Added order note successfully");

        }
    }



}
