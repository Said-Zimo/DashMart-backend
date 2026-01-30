using DashMart.Domain.Abstraction;
using DashMart.Domain.Validations;

namespace DashMart.Domain.People.Persons
{
    public class Person : Aggregate
    {
        public string FirstName { get; protected set; } = default!;
        public string LastName { get; protected set; } = default!;
        public Phone Phone1 { get; protected set; } = default!;
        public Phone? Phone2 { get; protected set; }
        public Email? Email { get; protected set; } 
        public bool IsActive { get; protected set; }
        public GenderEnum Gender { get; protected set; }
        public string PasswordHash { get; protected set; } = default!;
        public RoleEnum Role {  get; protected set; }

        protected readonly List<PersonAddressDetails> _addresses = new();
        public IReadOnlyCollection<PersonAddressDetails> Addresses => _addresses;

        protected Person() { }

        protected Person(string firstName, string lastName,string passwordHash , Phone phone1,Phone? phone2, Email? email, GenderEnum gender, RoleEnum role,
            PersonAddressDetails personAddressDetails)
        {
            UpdateFirstName(firstName);
            UpdateLastName(lastName);
            DomainValidation.EnsureValidString(passwordHash, 300, "Password");
            PasswordHash = passwordHash;
            Phone1 = phone1;
            if(phone2 != null) Phone2 = phone2;
            if(email  != null) Email = email;
            Gender = gender;
            Role = role;
            _addresses.Add(personAddressDetails);
            IsActive = true;
        }

        public void AddSecondPhoneNumber(string phoneNumber)
        {
            Phone2 = new Phone(phoneNumber);
        }

        public void UpdateMainPhoneNumber(string phoneNumber)
        {
            Phone1 = new Phone(phoneNumber);
        }

        public void UpdateSecondPhoneNumber(string phoneNumber)
        {
            Phone2 = new Phone(phoneNumber);
        }

        public void AddAddress(string title, int streetID, ushort addressType, string houseNumber, string? buildingNo = null)
        {
            if (_addresses.Any(x => x.StreetId == streetID && x.HouseNumber.ToLower() == houseNumber.ToLower().Trim()))
                throw new DomainException("Address is already used for this person");

            var address = PersonAddressDetails.Create(title, streetID, addressType, houseNumber, buildingNo);
            _addresses.Add(address);
        }

        public void DeleteAddress(int Id)
        {
            var address = _addresses.SingleOrDefault(a => a.Id == Id) ?? throw new DomainException("Address not found");

            _addresses.Remove(address);
        }

        public void UpdateFirstName(string firstName)
        {
            DomainValidation.EnsureValidString(firstName, 30, "First Name");
            FirstName = firstName;
        }

        public void UpdateLastName(string lastName)
        {
            DomainValidation.EnsureValidString(lastName, 30, "Last Name");
            LastName = lastName;
        }

        public void Deactivate() => IsActive = false;

        public void AddEmail(string email) => Email = new Email(email);
        
        public void UpdateEmail(string email) {
            Email = new Email(email);
        } 

        public PersonAddressDetails? FindPersonAddressByPublicId(Guid Id)
        {
            return _addresses.FirstOrDefault(x => x.PublicId == Id);
        }

    }

    public sealed record Phone
    {
        public string Value { get; private set; } = default!;

        private Phone() { }

        public Phone(string phoneNumber)
        {
           if (!DomainValidation.IsValidPhoneNumberFormat(phoneNumber))
               throw new DomainException("Phone number is invalid");
           Value = phoneNumber;
            
        }


        public static implicit operator string(Phone phone) => phone.Value;
    }

    public sealed record Email
    {
        public string Value { get; private set; } = default!;

        private Email() { }

        public Email(string email)
        {
            if (!DomainValidation.IsValidEmailFormat(email))
                 throw new DomainException("Email is not valid");
            
            Value = email!;
        }

        public override string ToString()=> $"{Value}";


        public static implicit operator string(Email email) => email.Value;
        
    }

}
