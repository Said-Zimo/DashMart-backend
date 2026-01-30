

using DashMart.Application.Auth;
using DashMart.Application.PasswordHashing;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Couriers;
using DashMart.Domain.People.Users;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Login.Courier
{
    public sealed record CourierLoginByPhoneNumberCommand
    (
        string PhoneNumber,
        string Password
        ) : IRequest<Result<string>>;

    public sealed class CourierLoginByPhoneNumberCommandValidator : AbstractValidator<CourierLoginByPhoneNumberCommand>
    {
        public CourierLoginByPhoneNumberCommandValidator()
        {
            RuleFor(x => x.PhoneNumber).NotNull().NotEmpty().WithMessage("Phone number cannot be empty or null")
                .Matches(@"^[0-9]+$").WithMessage("Phone number must be only numbers");

            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password cannot be empty or null");
        }
    }

    internal sealed class CourierLoginByPhoneNumberCommandHandler
        (ICourierRepository courierRepo, IPasswordHasher passwordHasher, IJwtTokenGenerator tokenGenerator)
        : IRequestHandler<CourierLoginByPhoneNumberCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CourierLoginByPhoneNumberCommand request, CancellationToken cancellationToken)
        {
            var courier = await courierRepo.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);

            if (courier == null)
                return Result<string>.Failure("Phone number/Password is not valid", StatusCodeEnum.Unauthorized);


            if(!passwordHasher.Verify(request.Password, courier.PasswordHash))
                return Result<string>.Failure("Phone number/Password is not valid", StatusCodeEnum.Unauthorized);

            if(!courier.IsActive)
                return Result<string>.Failure("Account is locked or inactive", StatusCodeEnum.Forbidden);

            var token = tokenGenerator.GenerateToken(courier.PublicId, courier.FirstName, RoleEnum.Courier, 0);


            return Result<string>.Success(token);


        }
    }

}
