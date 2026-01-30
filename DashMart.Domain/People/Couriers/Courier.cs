

using DashMart.Domain.Abstraction;
using DashMart.Domain.People.Couriers.DriverLicenses;
using DashMart.Domain.People.Persons;
using DashMart.Domain.Validations;

namespace DashMart.Domain.People.Couriers
{
    public class Courier : Person
    {
        public bool IsReadyToWork { get; private set; }


        private readonly List<DriverLicense> _driverLicenses = new();
        public IReadOnlyCollection<DriverLicense> DriverLicenses => _driverLicenses;


        private Courier() { }
        
        private Courier(string firstName, string lastName,Phone phone1,Phone? phone2, Email? email, DriverLicense driverLicense , string passwordHash,
                  GenderEnum gender, RoleEnum role, PersonAddressDetails address)
            : base(firstName,lastName,passwordHash, phone1, phone2, email, gender, role, address)
        {
            if (driverLicense == null) throw new DomainException("License cannot be null");
            _driverLicenses.Add(driverLicense);

        }

        public static Courier Create(string firstName, string lastName, string phoneNumber,string? phone2, string? email,
            DriverLicense driverLicense, string passwordHash, GenderEnum gender, RoleEnum role, PersonAddressDetails address)
        {
            var phone = new Phone(phoneNumber);

            Phone otherPhone;

            if (!string.IsNullOrWhiteSpace(phone2))
                otherPhone = new Phone(phone2);
            else
                otherPhone = null!;

            Email courierEmail;

            if (!string.IsNullOrWhiteSpace(email))
                courierEmail = new Email(email);
            else
                courierEmail = null!;

            return new Courier(firstName, lastName, phone, otherPhone, courierEmail, driverLicense, passwordHash, gender, role, address);
        }

        public void AddDriverLicense(string licenseNumber, string frontImagePath,
            string backImagePath, LicenseTypeEnum licenseType, DateOnly startDate, DateOnly expiryDate)
        {
            _driverLicenses.Add(DriverLicense.Create(licenseNumber, frontImagePath, backImagePath, licenseType, startDate, expiryDate,DateOnly.FromDateTime(DateTime.Now)));
        }

        public void ChangePassword(string passwordHash)
        {
            DomainValidation.EnsureValidString(passwordHash, 256, "Password Hash");
            PasswordHash = passwordHash;
        }

        public void ReadyToWork(bool isReady)
        {
            IsReadyToWork = isReady;
        }

        public void DeactivateLicense(int Id)
        {
            var license = _driverLicenses.SingleOrDefault(x=> x.Id == Id) ?? throw new DomainException("License not found");

            license.DeactivateLicense();
        }

        public void ReplaceDriverLicense(int Id, DriverLicense newLicense)
        {
            if (newLicense == null) throw new DomainException("Driver license cannot be null");

            var oldLicense = _driverLicenses.SingleOrDefault(x=> x.Id == Id) ?? throw new DomainException($"License with Id{Id} cannot be found");
            _driverLicenses.Remove(oldLicense);

            _driverLicenses.Add(newLicense);
        }
        
        public bool IsCourierLicense(int Id)
        {
            return _driverLicenses.Any(x => x.Id == Id);
        }

    }
}
