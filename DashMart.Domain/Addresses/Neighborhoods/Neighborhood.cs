using DashMart.Domain.Abstraction;
using DashMart.Domain.Addresses.Districts;
using DashMart.Domain.Addresses.Streets;
using DashMart.Domain.Validations;

namespace DashMart.Domain.Addresses.Neighborhoods
{
    public sealed class Neighborhood : Entity
    {
        public int DistrictId { get;private set; }
        public string Name { get; private set; } = default!;
        public District District { get; private set; } = default!;

        private readonly List<Street> _streets = new();
        public IReadOnlyCollection<Street> Streets => _streets;


        private Neighborhood() { }

        private Neighborhood(int districtId , string name)
        {
            if (districtId <= 0) throw new DomainException("Invalid District Id");

            DomainValidation.EnsureValidString(name, 100, "Neighborhood Name");
            DistrictId = districtId;
            Name = name;
        }

        public static Neighborhood Create(int districtId, string name)
        {
            return new Neighborhood(districtId, name);
        }

        public void UpdateName(string name)
        {
            DomainValidation.EnsureValidString(name, 100, "Neighborhood Name");
            Name= name;
        }
    }
}
