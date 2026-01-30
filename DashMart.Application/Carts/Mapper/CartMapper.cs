

using DashMart.Application.Carts.Query;
using DashMart.Domain.Carts;

namespace DashMart.Application.Carts.Mapper
{
    public static class CartMapper
    {
        public static IReadOnlyList<GetAllCartsQueryResponse> ToCartQueryResponse(IList<Cart> carts)
        {
            var list = new List<GetAllCartsQueryResponse>();


            foreach(var cart in carts)
            {
                list.Add(new GetAllCartsQueryResponse(cart.Customer.PublicId, cart.Id));
            }

            return list;
        }
    }
}
