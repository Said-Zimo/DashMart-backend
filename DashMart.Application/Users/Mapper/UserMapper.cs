

using DashMart.Application.Users.DTOs;
using DashMart.Domain.People.Users;

namespace DashMart.Application.Users.Mapper
{
    public static class UserMapper
    {
        public static UserViewDto? ToUserViewDTO(User? user)
        {
            if(user == null) return null;

            var permissions = new List<string>(user.GetPermissionsList());

            return new UserViewDto(user.PublicId, user.UserName, permissions);

        }

        public static IReadOnlyList<UserViewDto>ToListOfUserViewDTO(IList<User> users)
        {

            var list = new List<UserViewDto>();

            foreach (var user in users)
            {
                list.Add(ToUserViewDTO(user)!);
            }

            return list;

        }
            

    }
}
