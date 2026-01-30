using DashMart.Domain.Abstraction;
using DashMart.Domain.Addresses.Cities;
using DashMart.Domain.Addresses.Neighborhoods;
using DashMart.Domain.Validations;

namespace DashMart.Domain.Addresses.Districts
{
    public sealed class District : Entity
    {
        public int CityId { get; private set; }
        public string Name { get; private set; } = default!;
        public City City { get; private set; } = default!;

        private readonly List<Neighborhood> _neighborhoods = new();
        public IReadOnlyCollection<Neighborhood> Neighborhoods => _neighborhoods;


        private District() { }

        private District(int cityId, string name)
        {
            if (cityId <= 0) throw new DomainException("Invalid City ID");
            DomainValidation.EnsureValidString(name, 100, "District Name");
            CityId = cityId;
            Name = name;
        }

        public static District Create(int cityId, string name)
        {
            return new District(cityId, name);
        }

        public void UpdateName(string name)
        {
            DomainValidation.EnsureValidString(name, 100, "District Name");
            Name = name;
        }
    }
}
