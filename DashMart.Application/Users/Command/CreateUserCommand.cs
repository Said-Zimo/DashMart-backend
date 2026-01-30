using DashMart.Application.CurrentUserService;
using DashMart.Application.PasswordHashing;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Persons;
using DashMart.Domain.People.Users;
using DashMart.Domain.Addresses.Streets;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Users.Command
{
    public sealed record CreateUserCommand
    (
         string FirstName,
         string LastName,
         string Phone1,
         string? Phone2,
         string? Email,
         int Permissions ,
         string UserName ,
         string Password,

         bool Gender,
         string AddressTitle,
         Guid StreetId,
         ushort AddressType,
         string HouseNumber,
         string? BuildingNumber
        ) : IRequest<Result<string>>;



    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
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

            RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("User name cannot be empty or null")
                .MaximumLength(50).WithMessage("User name length must be less than 51 character");

            RuleFor(x => x.Permissions).GreaterThanOrEqualTo(0).WithMessage("Permissions value must be equal 0 or greater then 0");

            RuleFor(x => x.AddressTitle).NotNull().NotEmpty().WithMessage("Address title cannot be null or empty")
              .MaximumLength(30).WithMessage("Address title length must be less than 31");

            RuleFor(x => x.HouseNumber).NotNull().NotEmpty().WithMessage("House number cannot be null or empty")
                .MaximumLength(15).WithMessage("House number length must be less than 16");
        }
    }


    internal sealed class CreateUserCommandHandler
        (ICurrentUserService currentUser, IPasswordHasher passwordHasher, IUserRepository userRepo, IUnitOfWork unitOfWork, IStreetRepository streetRepo) : IRequestHandler<CreateUserCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.HasPermission(UserPermissionsEnum.CreatePerson))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            if (await userRepo.IsExistByPhoneNumberAsync(request.Phone1, cancellationToken))
                return Result<string>.Failure($"User already exist with this phone number {request.Phone1}", StatusCodeEnum.Conflict);

            var street = await streetRepo.GetStreetDetailsByPublicIdAsync(request.StreetId , cancellationToken);

            if (street == null)
                return "Street not found";

            var address = PersonAddressDetails.Create(request.AddressTitle, street.Id, request.AddressType, request.HouseNumber, request.BuildingNumber);

            var passwordHash = passwordHasher.Hash(request.Password);

            var gender = (request.Gender) ? GenderEnum.Female : GenderEnum.Male;

            var user = User.Create(request.FirstName, request.LastName, request.Phone1, request.Phone2, request.Email, request.Permissions, 
                request.UserName, passwordHash, gender, RoleEnum.User, address);
              
            userRepo.Add(user);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("User created successfully");

        }
    }


}
