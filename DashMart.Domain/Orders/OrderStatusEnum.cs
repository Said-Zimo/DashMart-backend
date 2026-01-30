

namespace DashMart.Domain.Orders
{
    public enum OrderStatusEnum
    {
        Pending = 1,            
        Confirmed = 2,          

       
        ReadyForPickup = 3, 
        CourierAssigned = 4,

       
        PickedUp = 5,      
        OutForDelivery = 6,

      
        Delivered = 7,        


        Cancelled = 8,        
        Returned = 9,         
        DeliveryFailed = 10   
    }
}
