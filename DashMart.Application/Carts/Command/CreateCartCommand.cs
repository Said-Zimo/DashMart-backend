using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Customers;
using DashMart.Domain.People.Users;
using DashMart.Domain.Carts;
using DashMart.Domain.UnitOfWorks;
using MediatR;

namespace DashMart.Application.Carts.Command
{
    public sealed record CreateCartCommand
    (
        Guid CustomerId
        ) : IRequest<Result<string>>;



    internal sealed class CreateCartCommandHandler
        (ICurrentUserService currentUser, ICartRepository cartRepo ,ICustomerRepository customerRepo, IUnitOfWork unitOfWork)
        : IRequestHandler<CreateCartCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            var customer = await customerRepo.GetByPublicIdAsync(request.CustomerId);

            if (customer == null)
                return Result<string>.Failure("Customer not found", StatusCodeEnum.NotFound);

            var isOwner = customer.PublicId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.AccessCart);

            if (!isOwner && !isUser)
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            if (await cartRepo.IsExistCart(customer.Id))
                return Result<string>.Failure($"Cart already existing from customer with Id {request.CustomerId}", StatusCodeEnum.Conflict);

            var cart = Cart.Create(customer.Id);

            cartRepo.Add(cart);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Cart created successfully");
        }
    }


}
