

using Isopoh.Cryptography.Argon2;
using DashMart.Application.PasswordHashing;

namespace DashMart.Infrastructure.PasswordHashing
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return Argon2.Hash(password);
        }

        public bool Verify(string password, string hashedPassword)
        {
            return Argon2.Verify(hashedPassword.Trim(), password);
        }
    }
}
