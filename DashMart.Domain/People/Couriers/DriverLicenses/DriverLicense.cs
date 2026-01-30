


using DashMart.Domain.Abstraction;
using DashMart.Domain.Validations;

namespace DashMart.Domain.People.Couriers.DriverLicenses
{
    public sealed class DriverLicense : Entity
    {
        public int CourierId { get;private set; }
        public Courier Courier { get; private set; } = default!;
        public string LicenseNumber { get; private set; } = default!;
        public LicenseTypeEnum LicenseType { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly ExpiryDate { get; private set; }
        public bool IsEnabled { get; private set; }
        public string FrontImagePath { get; private set; } = default!;
        public string BackImagePath { get; private set; } = default!;
        private DriverLicense() { }

        private DriverLicense(string licenseNumber, string frontImagePath,
            string backImagePath, LicenseTypeEnum licenseType, DateOnly startDate, DateOnly expiryDate,DateOnly currentDate, bool isEnabled)
        {
            DomainValidation.EnsureValidString(licenseNumber, 11, "License Number");
            DomainValidation.EnsureValidString(frontImagePath, 300, "Front image path");
            DomainValidation.EnsureValidString(backImagePath, 300, "Back image path");

            if (startDate > expiryDate)
                throw new DomainException("Start date must be before expiry date.");

            if (startDate > currentDate) throw new DomainException("License start date cannot be in the future");
            if (expiryDate < currentDate) throw new DomainException("License Expired");

            LicenseNumber = licenseNumber;
            FrontImagePath = frontImagePath;
            BackImagePath = backImagePath;
            StartDate = startDate;
            ExpiryDate = expiryDate;
            LicenseType = licenseType;
            IsEnabled = isEnabled;
        }

        public static DriverLicense Create(string licenseNumber, string frontImagePath,
            string backImagePath, LicenseTypeEnum licenseType, DateOnly startDate, DateOnly expiryDate , DateOnly currentDate)
        {
            return new DriverLicense(licenseNumber , frontImagePath , backImagePath,
                licenseType, startDate, expiryDate, currentDate, true);

        }
        public void DeactivateLicense()
        {
            IsEnabled = false;
        }
    }
}
