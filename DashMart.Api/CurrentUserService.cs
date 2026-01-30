using DashMart.Application.CurrentUserService;
using DashMart.Domain.People;
using DashMart.Domain.People.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DashMart.Api
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor context;

        public CurrentUserService(IHttpContextAccessor Context)
        {
            this.context = Context;
        }

        public Guid? UserID {
            get
            {
                var sub = context?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                return Guid.TryParse(sub, out Guid Id) ? Id : null;
            }
        }

        public bool IsAuthenticated => context?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public IList<RoleEnum> Roles
        { 
            get
            {
                var roles = context?.HttpContext?.User?.FindAll(ClaimTypes.Role).ToList();

                var validRoles = new List<RoleEnum>();

                if (roles == null) return validRoles;

                foreach (var role in roles)
                {
                    if(Enum.TryParse<RoleEnum>(role.Value, out RoleEnum rolesEnum))
                        validRoles.Add(rolesEnum);
                }

                return validRoles;
            }
        }

        public long Permissions {
            get
            {
                var permissions = context?.HttpContext?.User?.FindFirstValue("Permissions");

                if(int.TryParse(permissions, out int permission))
                    return permission;
                else
                    return 0;
            }
        }

        public bool HasPermission(UserPermissionsEnum required)
        {
            return ((UserPermissionsEnum)Permissions & required) == required;
        }
    }
}
