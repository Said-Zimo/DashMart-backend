using DashMart.Application.Auth;
using DashMart.Domain.People;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DashMart.Infrastructure.Auth
{
    public sealed class JwtTokenGenerator
        (IConfiguration configuration) : IJwtTokenGenerator
    {

        public string GenerateToken(Guid userId, string userName, RoleEnum Role, int permissions)
        {

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Sub, userId.ToString()),
                new (ClaimTypes.Name, userName),
                new (ClaimTypes.Role, Role.ToString()),
                new ("Permissions", permissions.ToString())
            };

            var key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["DashMart_JWT_SECRET_KEY"]!));
            var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                   issuer: configuration["JWT:Issuer"],
                   audience: configuration["JWT:Audience"],
                   claims: claims,
                   notBefore: DateTime.UtcNow,
                   expires: DateTime.Now.AddMinutes(30),
                   signingCredentials: sc
                );


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
