

using DashMart.Domain.People;
using DashMart.Domain.People.Users;

namespace DashMart.Application.CurrentUserService
{
    public interface ICurrentUserService
    {
        Guid? UserID { get; }
        bool IsAuthenticated { get; }
        IList<RoleEnum> Roles { get; }
        long Permissions { get; }
        bool HasPermission(UserPermissionsEnum required);
    }
}
