using DashMart.Domain.Abstraction;
using DashMart.Domain.Addresses.Countries;
using DashMart.Domain.Addresses.Districts;
using DashMart.Domain.Validations;

namespace DashMart.Domain.Addresses.Cities
{
    public sealed class City : Entity
    {
        public int CountryId { get; private set; }
        public string Name { get; private set; } = default!;
        public Country Country { get; private set; } = default!;

        private readonly List<District> _districts = new();
        public IReadOnlyCollection<District> Districts => _districts;

        private City() { }

        private City(int countryId, string name)
        {
            if (countryId <= 0) throw new DomainException("Invalid Country ID");

            DomainValidation.EnsureValidString(name, 100, "City Name");
            CountryId = countryId;
            Name = name;
        }

        public static City Create(int countryId, string name)
        {
            return new City(countryId, name);
        }

        public void UpdateName(string name)
        {
            DomainValidation.EnsureValidString(name, 100, "City Name");
            Name = name;
        }
    }
}
