
using DashMart.Application.CurrentUserService;
using DashMart.Application.Results;
using DashMart.Domain.People.Couriers;
using DashMart.Domain.People.Couriers.DriverLicenses;
using DashMart.Domain.People.Users;
using DashMart.Domain.UnitOfWorks;
using MediatR;

namespace DashMart.Application.Couriers.Command
{
    public sealed record DeactivateDriverLicenseCommand
    (
        Guid CourierId,
        string DriverLicenseNumber
        ) : IRequest<Result<string>>;


    internal sealed class DeactivateDriverLicenseCommandHandler
        (ICurrentUserService currentUser, ICourierRepository courierRepo, IDriverLicenseRepository driverLicenseRepo, IUnitOfWork unitOfWork) 
        : IRequestHandler<DeactivateDriverLicenseCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeactivateDriverLicenseCommand request, CancellationToken cancellationToken)
        {
            
            if (!currentUser.HasPermission(UserPermissionsEnum.UpdatePerson))
                return Result<string>.Failure("Access Denied", StatusCodeEnum.Forbidden);

            var courier = await courierRepo.GetByPublicIdAsync(request.CourierId, cancellationToken);

            if (courier == null)
                return Result<string>.Failure("Courier not found", StatusCodeEnum.NotFound);

            var driverLicense = await driverLicenseRepo.GetDriverLicenseByPublicIdAsync(request.DriverLicenseNumber, cancellationToken);

            if (driverLicense == null)
                return Result<string>.Failure("Driver license not found", StatusCodeEnum.NotFound);

            courier.DeactivateLicense(driverLicense.Id);

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return Result<string>.Success("Courier driver license Deactivated successfully");
        }
    }

}
