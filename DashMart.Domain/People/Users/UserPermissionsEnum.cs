

namespace DashMart.Domain.People.Users
{
    [Flags]
    public enum UserPermissionsEnum
    {
        None = 0,

        ShowPerson = 1,
        UpdatePerson = 2,
        DeletePerson = 4,
        CreatePerson = 8,

        ShowOrders = 16,
        UpdateOrder = 32,
        DeleteOrder = 64,
        CreateOrder = 128,

        ShowProducts = 256,
        UpdateProducts = 512,
        DeleteProducts = 1024,
        CreateProduct = 2048,

        AccessCategory = 4096,

        AccessCart = 8192
    }
}
