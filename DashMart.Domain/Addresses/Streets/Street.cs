using DashMart.Domain.Abstraction;
using DashMart.Domain.Addresses.Neighborhoods;
using DashMart.Domain.Validations;

namespace DashMart.Domain.Addresses.Streets
{
    public sealed class Street : Entity
    {
        public string Name { get;private set; } = default!;
        public int NeighborhoodID { get; private set; }
        public Neighborhood Neighborhood { get; private set; } = default!;

        private Street() { }

        private Street(int neighborhoodId ,string name)
        {
            if (neighborhoodId <= 0) throw new DomainException("Invalid Neighborhood ID");

            DomainValidation.EnsureValidString(name, 100, "Street Name");

            Name = name;
            NeighborhoodID = neighborhoodId;
        }


        public static Street Create(int neighborhoodId,string name )
        {
            return new Street(neighborhoodId,name);
        }

        public void UpdateName(string name)
        {
            DomainValidation.EnsureValidString(name, 100, "Street Name");
            Name = name;
        }

    }
}
