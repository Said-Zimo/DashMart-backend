using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Customers;
using DashMart.Domain.People.Users;
using DashMart.Domain.Carts;
using DashMart.Domain.Orders;
using DashMart.Domain.UnitOfWorks;
using MediatR;

namespace DashMart.Application.Customers.Command
{
    public sealed record PlaceCustomerOrderCommand
    (
        Guid CustomerId,
        Guid SelectedAddressId,
        string? Note
        
        ) : IRequest<Result<string>>;


    internal sealed class PlaceCustomerOrderCommandHandler
        (ICurrentUserService currentUser, ICustomerRepository customerRepo, ICartRepository cartRepo, IUnitOfWork unitOfWork) 
        : IRequestHandler<PlaceCustomerOrderCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(PlaceCustomerOrderCommand request, CancellationToken cancellationToken)
        {
            var customer = await customerRepo.GetByPublicIdAsync( request.CustomerId, cancellationToken );

            if (customer == null) return Result<string>.Failure("Customer not found", StatusCodeEnum.NotFound);

            var isOwner = customer.PublicId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.CreateOrder);

            if (!isOwner && !isUser)
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var selectedAddress = customer.FindPersonAddressByPublicId(request.SelectedAddressId);

            if (selectedAddress == null) return Result<string>.Failure("Address not belongs to this customer", StatusCodeEnum.BadRequest);

            var shippingCart = await cartRepo.GetActiveShippingCartByCustomerIdAsync(customer.Id, cancellationToken);

            if (shippingCart == null) return Result<string>.Failure("This customer does not have active shipping cart", StatusCodeEnum.NotFound);


            var newOrder = Order.Create(customer.Id, selectedAddress.StreetId , selectedAddress.BuildingNo , selectedAddress.HouseNumber);

            if(request.Note != null)
               newOrder.SetNote(request.Note);

            foreach (var item in shippingCart.CartItems)
            {
                newOrder.AddItem(item.ProductId, item.Quantity,item.Price );
            }

            cartRepo.Delete(shippingCart);

            customer.PlaceOrder(newOrder);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Customer placed order successfully");
        }
    }
}
