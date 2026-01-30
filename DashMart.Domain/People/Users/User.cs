using DashMart.Domain.Abstraction;
using DashMart.Domain.People.Persons;
using DashMart.Domain.Validations;

namespace DashMart.Domain.People.Users
{
    public class User : Person
    {
        public int Permissions { get; private set; }
        public string UserName { get; private set; } = default!;

        private User() { }

        private User(string firstName, string lastName, Phone phoneNumber, Phone? phone2, Email? email, int permissions , string userName, string passwordHash,
            GenderEnum gender,RoleEnum role, PersonAddressDetails address) 
            : base(firstName,lastName,passwordHash, phoneNumber,phone2, email, gender, role,address) 
        {
            if (permissions < 0 ) throw new DomainException("Permissions not valid");
            DomainValidation.EnsureValidString(userName, 100, "User Name");
            DomainValidation.EnsureValidString(passwordHash, 250, "Password");

            UserName = userName;
            Permissions = permissions;
        }

        public static User Create(string firstName, string lastName, string phoneNumber, string? phone2, string? email, int permissions, string userName, string passwordHash,
            GenderEnum gender,RoleEnum role, PersonAddressDetails address)
        {
            var phone = new Phone(phoneNumber);

            Phone otherPhone;

            if (!string.IsNullOrWhiteSpace(phone2))
                otherPhone = new Phone(phone2);
            else
                otherPhone = null!;

            Email userEmail;

            if (!string.IsNullOrWhiteSpace(email))
                userEmail = new Email(email);
            else
                userEmail = null!;

            return new User(firstName, lastName, phone, otherPhone, userEmail, permissions, userName, passwordHash, gender, role, address);
        }

        public void AddNewPermissions(UserPermissionsEnum userPermissions)
        {
            Permissions |= (int)userPermissions;
        }

        public void ReductionUserPermissions()
        {
            Permissions = 0;
        }

        public IList<string> GetPermissionsList()
        {
            UserPermissionsEnum userPermissions = (UserPermissionsEnum)Permissions;

            return Enum.GetValues<UserPermissionsEnum>()
                .Where(x => x.HasFlag(x) && x != UserPermissionsEnum.None)
                .Select(x => x.ToString())
                .ToList();
        }


    }
}
