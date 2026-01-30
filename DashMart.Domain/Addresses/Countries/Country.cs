using DashMart.Domain.Abstraction;
using DashMart.Domain.Addresses.Cities;
using DashMart.Domain.Validations;

namespace DashMart.Domain.Addresses.Countries
{
    public sealed class Country : Entity
    {
        public string Name { get; private set; } = default!;

        private readonly List<City> _cities = new();
        public IReadOnlyCollection<City> Cities => _cities;

        private Country() { }

        private Country(string name)
        {
            DomainValidation.EnsureValidString(name, 100, "Country Name");
            Name = name;
        }

        public static Country Create( string name)
        {
            return new Country(name);
        }

        public void UpdateName(string name)
        {
            DomainValidation.EnsureValidString(name, 100, "Country Name");
            Name = name;
        }
    }
}
