using DashMart.Domain.Abstraction;
using DashMart.Domain.Addresses;
using DashMart.Domain.Addresses.Streets;
using DashMart.Domain.Validations;

namespace DashMart.Domain.People.Persons
{
    public sealed class PersonAddressDetails : Entity
    {
        public int PersonId { get; private set; }
        public Person Person { get; private set; } = default!;
        public string Title { get; private set; } = default!;
        public int StreetId { get; private set; }
        public Street Street { get; private set; } = default!;
        public AddressTypeEnum AddressType { get; private set; }
        public string? BuildingNo { get; private set; }
        public string HouseNumber { get; private set; } = default!;
        
        private PersonAddressDetails() { }

        private PersonAddressDetails( string title, int streetID, AddressTypeEnum addressType, string houseNumber, string? buildingNo = null)
        {

            if (streetID <= 0) throw new DomainException("Street ID must be positive");

            ValidateHouseAndBuilding(addressType, houseNumber, buildingNo);

            Title = string.IsNullOrWhiteSpace(title) ? "Unknown" : title.Length > 30 ? title[..29] : title;
            StreetId = streetID;
            AddressType = addressType;
            HouseNumber = houseNumber.Trim();
            BuildingNo = buildingNo?.Trim();
        }

        public static PersonAddressDetails Create( string title, int streetID, ushort addressType, string houseNumber, string? buildingNo = null)
        {
            if (addressType > 4) addressType = 4;

            return new PersonAddressDetails( title, streetID, (AddressTypeEnum)addressType, houseNumber, buildingNo);
        }

        private void ValidateHouseAndBuilding(AddressTypeEnum type, string houseNo, string? bldNo)
        {

            DomainValidation.EnsureValidString(houseNo, 15, "House Number");

            if ((type == AddressTypeEnum.HouseInBuilding || type == AddressTypeEnum.OfficeInBuilding) && string.IsNullOrWhiteSpace(bldNo))
            {
                throw new DomainException("Building number is required for this address type.");
            }

            if (!string.IsNullOrWhiteSpace(bldNo))
                DomainValidation.EnsureValidString(bldNo, 15, "Building Number");
        }

        
        public void SetHouseNumber(AddressTypeEnum type, string houseNumber, string? bldNo)
        {
            ValidateHouseAndBuilding(type, houseNumber, bldNo);

            HouseNumber = houseNumber;

            if (bldNo != null)
            {
                BuildingNo = bldNo;
            }
        }

        public void SetBuildingNumber(string buildingNumber)
        {
            DomainValidation.EnsureValidString(buildingNumber, 15, "Building Number");

            BuildingNo = buildingNumber;
        }

    }

}
