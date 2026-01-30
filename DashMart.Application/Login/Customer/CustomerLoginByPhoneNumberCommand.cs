

using DashMart.Application.Auth;
using DashMart.Application.PasswordHashing;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.xCustomer;
using DashMart.Domain.People.Users;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Login.Customer
{
    public sealed record CustomerLoginByPhoneNumberCommand
    (
        string PhoneNumber,
        string Password
        ) : IRequest<Result<string>>;

    public sealed class CustomerLoginByPhoneNumberCommandValidator : AbstractValidator<CustomerLoginByPhoneNumberCommand>
    {
        public CustomerLoginByPhoneNumberCommandValidator()
        {
            RuleFor(x => x.PhoneNumber).NotNull().NotEmpty().WithMessage("Phone number cannot be empty or null")
                .Matches(@"^[0-9]+$").WithMessage("Phone number must be only numbers");

            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password cannot be empty or null");
        }
    }

    internal sealed class CustomerLoginByPhoneNumberCommandHandler
        (ICustomerRepository customerRepo, IPasswordHasher passwordHasher, IJwtTokenGenerator tokenGenerator) : IRequestHandler<CustomerLoginByPhoneNumberCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CustomerLoginByPhoneNumberCommand request, CancellationToken cancellationToken)
        {
            var customer = await customerRepo.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);

            if (customer == null)
                return Result<string>.Failure("Phone number/Password is not valid", StatusCodeEnum.Unauthorized);

            if (!passwordHasher.Verify(request.Password, customer.PasswordHash))
                return Result<string>.Failure("Phone number/Password is not valid", StatusCodeEnum.Unauthorized);

            if(!customer.IsActive)
                return Result<string>.Failure("Account is locked or inactive", StatusCodeEnum.Forbidden);

            var token = tokenGenerator.GenerateToken(customer.PublicId, customer.FirstName, RoleEnum.Customer, 0);

            return Result<string>.Success(token);

        }
    }
}
