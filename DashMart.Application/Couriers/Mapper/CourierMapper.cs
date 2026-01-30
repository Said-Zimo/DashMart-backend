

using DashMart.Application.Couriers.DTOs;
using DashMart.Domain.People.Couriers;

namespace DashMart.Application.Couriers.Mapper
{
    public static class CourierMapper
    {
        public static CourierViewDto? ToCourierViewDTO(Courier? courier)
        {
            if(courier == null) return null;

            return new CourierViewDto(courier.PublicId, courier.IsReadyToWork, courier.FirstName, courier.LastName, courier.Phone1);
        }

        public static IReadOnlyList<CourierViewDto> ToListOfCourierViewDTO(IList<Courier> couriers)
        {
            var list = new List<CourierViewDto>();

            foreach(var courier in couriers)
            {
                list.Add(ToCourierViewDTO(courier)!);
            }

            return list;
        }

    }
}
