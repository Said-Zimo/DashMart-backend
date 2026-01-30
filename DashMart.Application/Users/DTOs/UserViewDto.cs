

namespace DashMart.Application.Users.DTOs
{
    public sealed record UserViewDto
    (
        Guid UserId,
        string UserName,
        ICollection<string> Permissions 
       );


}
