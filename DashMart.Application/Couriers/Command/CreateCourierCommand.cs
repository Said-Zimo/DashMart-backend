using DashMart.Application.PasswordHashing;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Couriers;
using DashMart.Domain.People.Couriers.DriverLicenses;
using DashMart.Domain.People.Persons;
using DashMart.Domain.Addresses.Streets;
using DashMart.Domain.UnitOfWorks;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Couriers.Command
{
    public sealed record CreateCourierCommand
    (
        string FirstName,
        string LastName, 
        string PhoneNumber,
        string? Phone2,
        string? Email,

        string LicenseNumber,
        string FrontImagePath,
        string BackImagePath,
        ushort LicenseType,
        DateOnly StartDate,
        DateOnly ExpiryDate,

        string Password,
        bool Gender,
        string AddressTitle,
        Guid StreetID,
        ushort AddressType,
        string HouseNumber,
        string? BuildingNo = null
        ) : IRequest<Result<string>>;


    public sealed class CreateCourierCommandValidator : AbstractValidator<CreateCourierCommand>
    {
        public CreateCourierCommandValidator()
        {
            RuleFor(x=> x.FirstName).NotNull().NotEmpty().WithMessage("Fist name cannot be empty or null")
                .MaximumLength(30).WithMessage("First name length must be less than 31 character");

            RuleFor(x=> x.LastName).NotNull().NotEmpty().WithMessage("Last name cannot be empty or null")
                .MaximumLength(30).WithMessage("Last name length must be less than 31 character");

            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Password cannot be null or empty")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .Matches(@"[@!#$%^&*_+=*`~\|]").WithMessage("Password must contain at least one special character.");

            RuleFor(x=> x.PhoneNumber).NotNull().NotEmpty().WithMessage("Phone number cannot be empty or null")
                .Matches(@"^[0-9]+$").WithMessage("Phone number must be only numbers")
                .Length(11).WithMessage("Phone number must be 11 number");


            RuleFor(x=> x.LicenseNumber).NotNull().NotEmpty().WithMessage("Driving license number cannot be empty or null")
                .Matches(@"^[0-9]+$").WithMessage("Driving license number must be only numbers")
                .Length(11).WithMessage("Driving license name length must be 11 number");

            RuleFor(x => (LicenseTypeEnum)x.LicenseType).IsInEnum().WithMessage("License type is not valid");

            RuleFor(x=> x.FrontImagePath).NotNull().NotEmpty().WithMessage("Front image cannot be empty or null")
                .MaximumLength(300).WithMessage("Front image path length must be less than 300 character");

            RuleFor(x=> x.BackImagePath).NotNull().NotEmpty().WithMessage("Back image path cannot be empty or null")
                .MaximumLength(300).WithMessage("Back image path length must be less than 300 character");

            RuleFor(x => x.StartDate).LessThan(x => x.ExpiryDate).WithMessage("Expiry date cannot be before start date");

            RuleFor(x=> x.AddressTitle).NotNull().NotEmpty().WithMessage("Address title cannot be null or empty")
                .MaximumLength(30).WithMessage("Address title length must be less than 31");

            RuleFor(x => x.HouseNumber).NotNull().NotEmpty().WithMessage("House number cannot be null or empty")
                .MaximumLength(15).WithMessage("House number length must be less than 16");
        }
    }

    internal sealed class CreateCourierCommandHandler
        (ICourierRepository courierRepo, IDriverLicenseRepository driverLicenseRepo, 
        IStreetRepository streetRepo,IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
        : IRequestHandler<CreateCourierCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateCourierCommand request, CancellationToken cancellationToken)
        {

            if (await courierRepo.IsExistsByPhoneNumberAsync(request.PhoneNumber , cancellationToken))
                return Result<string>.Failure("Courier is already exists", StatusCodeEnum.Conflict);

            if (await driverLicenseRepo.IsExistByLicenseNumber(request.LicenseNumber, cancellationToken))
                return Result<string>.Failure("driver license is already exists by another courier", StatusCodeEnum.BadRequest);

            LicenseTypeEnum licenseType = (LicenseTypeEnum)request.LicenseType;

            var license = DriverLicense.Create(request.LicenseNumber, request.FrontImagePath,
                request.BackImagePath, licenseType, request.StartDate, request.ExpiryDate, DateOnly.FromDateTime(DateTime.Now));

            var street = await streetRepo.GetStreetDetailsByPublicIdAsync(request.StreetID , cancellationToken);

            if (street == null) return Result<string>.Failure("Street not found", StatusCodeEnum.NotFound);

            var personAddress = PersonAddressDetails.Create(request.AddressTitle, street.Id, request.AddressType, request.HouseNumber, request.BuildingNo);

            var gender = (request.Gender) ? GenderEnum.Female : GenderEnum.Male;

            var hashedPass = passwordHasher.Hash(request.Password);

            var courier = Courier.Create(request.FirstName, request.LastName, request.PhoneNumber, request.Phone2, request.Email, license, hashedPass, gender,RoleEnum.Courier, personAddress);

            courierRepo.Add(courier);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Courier created successfully");
            
        }
    }



}
