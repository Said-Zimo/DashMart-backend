

using DashMart.Domain.People;
using DashMart.Domain.People.Users;

namespace DashMart.Application.Auth
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId, string userName, RoleEnum Role, int permissions);
    }
}
