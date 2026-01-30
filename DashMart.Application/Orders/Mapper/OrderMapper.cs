

using DashMart.Application.Orders.DTOs;
using DashMart.Domain.Orders;

namespace DashMart.Application.Orders.Mapper
{
    public static class OrderMapper
    {

        public static OrderViewDto? ToOrderViewDTO(Order? order)
        {
            if(order == null) return null;


            return new OrderViewDto(order.CustomerId, order.CourierId, order.Note, order.StreetId, order.BuildingNo, order.HouseNo);

        }

        public static IReadOnlyList<OrderViewDto> ToListOfOrderViewDTO(IList<Order> orders)
        {
            var list = new List<OrderViewDto>();

            foreach (var order in orders)
            {
                list.Add(ToOrderViewDTO(order)!);
            }

            return list;

        }

    }
}
