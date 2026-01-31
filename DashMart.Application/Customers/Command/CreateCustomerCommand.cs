using DashMart.Application.PasswordHashing;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Customers;
using DashMart.Domain.People.Persons;
using DashMart.Domain.Addresses.Streets;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Customers.Command
{
    public sealed record CreateCustomerCommand
    (
         string FirstName ,
         string LastName,
         string Password,
         string Phone1 ,
         string? Phone2 ,
         string? Email ,
         bool Gender,
         string AddressTitle,
         Guid StreetId,
         ushort AddressType,
         string HouseNumber,
         string ? BuildingNumber

    ) : IRequest<Result<string>>;

    public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("Fist name cannot be empty or null")
                .MaximumLength(30).WithMessage("First name length must be less than 31 character");

            RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Last name cannot be empty or null")
                .MaximumLength(30).WithMessage("Last name length must be less than 31 character");

            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Password cannot be null or empty")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .Matches(@"[@!#$%^&*_+=*`~\|]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.Phone1).NotNull().NotEmpty().WithMessage("Phone number cannot be empty or null")
                .Matches(@"^[0-9]+$").WithMessage("Phone number must be only numbers")
                .Length(11).WithMessage("Phone number must be 11 number");

            RuleFor(x => x.AddressTitle).NotNull().NotEmpty().WithMessage("Address title cannot be null or empty")
               .MaximumLength(30).WithMessage("Address title length must be less than 31");

            RuleFor(x => x.HouseNumber).NotNull().NotEmpty().WithMessage("House number cannot be null or empty")
                .MaximumLength(15).WithMessage("House number length must be less than 16");

        }
    }
    internal sealed class CreateCustomerCommandHandler
        (ICustomerRepository customerRepo,IStreetRepository streetRepo, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork) : IRequestHandler<CreateCustomerCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {

            var existing = await customerRepo.IsExistsByPhoneNumber(request.Phone1);

            if (existing)
                return Result<string>.Failure("Customer is already exists", StatusCodeEnum.Conflict);

            var street = await streetRepo.GetStreetDetailsByPublicIdAsync(request.StreetId, cancellationToken);

            if (street == null) return Result<string>.Failure("Street not found", StatusCodeEnum.NotFound);

            var addressDetails = PersonAddressDetails.Create(request.AddressTitle, street.Id, request.AddressType, request.HouseNumber, request.BuildingNumber);

            var gender = (request.Gender) ? GenderEnum.Female: GenderEnum.Male;

            var hashedPass = passwordHasher.Hash(request.Password);

            Customer customer = Customer.Create(request.FirstName, request.LastName, hashedPass, request.Phone1,request.Phone2, request.Email, gender, RoleEnum.Customer, addressDetails);

            customerRepo.Add(customer);

            await unitOfWork.SaveChangeAsync(cancellationToken);
            
            return Result<string>.Success("Customer created successfully");
        }
    }

}
