using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.xCustomer;
using DashMart.Domain.People.Users;
using DashMart.Domain.Addresses.Streets;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Customers.Command
{
    public sealed record AddCustomerAddressCommand
    (
        Guid CustomerId,
        string AddressTitle,
        Guid StreetID,
        ushort AddressType,
        string HouseNumber,
        string? BuildingNo = null
        ) : IRequest<Result<string>>;


    public sealed class AddCustomerAddressCommandValidator : AbstractValidator<AddCustomerAddressCommand>
    {
        public AddCustomerAddressCommandValidator()
        {
            RuleFor(x => x.AddressTitle).NotNull().NotEmpty().WithMessage("Address title cannot be null or empty")
                            .MaximumLength(30).WithMessage("Address title length must be less than 31");

            RuleFor(x => x.HouseNumber).NotNull().NotEmpty().WithMessage("House number cannot be null or empty")
                .MaximumLength(15).WithMessage("House number length must be less than 16");
        }
    }

    internal sealed class AddCustomerAddressCommandHandler
        (ICurrentUserService currentUser, ICustomerRepository customerRepo,IStreetRepository streetRepo, IUnitOfWork unitOfWork) : IRequestHandler<AddCustomerAddressCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(AddCustomerAddressCommand request, CancellationToken cancellationToken)
        {   
            var customer = await customerRepo.GetByPublicIdAsync(request.CustomerId);

            if (customer == null) return Result<string>.Failure("Customer not found", StatusCodeEnum.NotFound);

            var isOwner = customer.PublicId == currentUser.UserID;
            var isUser = currentUser.Roles.Contains(RoleEnum.User) && currentUser.HasPermission(UserPermissionsEnum.UpdatePerson);

            if (!isOwner && !isUser)
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var street = await streetRepo.GetStreetDetailsByPublicIdAsync(request.StreetID , cancellationToken);

            if (street == null) return Result<string>.Failure("Street not found", StatusCodeEnum.NotFound);

            customer.AddAddress(request.AddressTitle, street.Id , request.AddressType , request.HouseNumber , request.BuildingNo);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Customer address added successfully");

        }
    }



}
