using DashMart.Application.Auth;
using DashMart.Application.PasswordHashing;
using DashMart.Application.Results;
using DashMart.Domain.People;
using DashMart.Domain.People.Users;
using FluentValidation;
using MediatR;

namespace DashMart.Application.Login.User
{
    public sealed record UserLoginByPhoneNumberCommand
    (
        string PhoneNumber,
        string Password
        ) : IRequest<Result<string>>;


    public sealed class UserLoginByPhoneNumberCommandValidator : AbstractValidator<UserLoginByPhoneNumberCommand>
    {
        public UserLoginByPhoneNumberCommandValidator()
        {
            RuleFor(x => x.PhoneNumber).NotNull().NotEmpty().WithMessage("Phone number cannot be empty or null")
                .Matches(@"^[0-9]+$").WithMessage("Phone number must be only numbers");

            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password cannot be empty or null");

        }
    }


    internal sealed class UserLoginByPhoneNumberCommandHandler
        (IUserRepository userRepo, IPasswordHasher passwordHasher, IJwtTokenGenerator tokenGenerator) : IRequestHandler<UserLoginByPhoneNumberCommand ,Result<string>>
    {
        public async Task<Result<string>> Handle(UserLoginByPhoneNumberCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepo.GetUserByPhoneNumberAsync(request.PhoneNumber, cancellationToken);

            if (user == null)
                return Result<string>.Failure("Phone number/Password is not valid", StatusCodeEnum.Unauthorized);

            if (!passwordHasher.Verify(request.Password,user.PasswordHash))
                return Result<string>.Failure("Phone number/Password is not valid", StatusCodeEnum.Unauthorized);

            if (!user.IsActive)
                return Result<string>.Failure("Account is locked or inactive", StatusCodeEnum.Forbidden);

            var token = tokenGenerator.GenerateToken(user.PublicId, user.UserName, RoleEnum.User, user.Permissions);

            return Result<string>.Success(token);
        }
    }
}
