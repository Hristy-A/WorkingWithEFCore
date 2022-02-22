using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Store.Infrastructure.HashProviders
{
    public class VanillaHashProvider : IPasswordHashProvider
    {
        public string GenerateHash([DisallowNull] string input)
        {
            byte[] salt = new byte[16];
            byte[] hashedPassword = new byte[36];
            RandomNumberGenerator.Fill(salt);

            using (var rfc = new Rfc2898DeriveBytes(input, salt, 10000))
            {
                byte[] hash = rfc.GetBytes(20);
                Array.Copy(hash, 0, hashedPassword, 0, 20);
                Array.Copy(salt, 0, hashedPassword, 20, 16);
            }

            return Convert.ToBase64String(hashedPassword);
        }

        public bool Verify(string input, string hash)
        {
            byte[] hashedPassword = Convert.FromBase64String(hash);
            byte[] salt = new byte[16];
            byte[] hashedInput;

            Array.Copy(hashedPassword, 20, salt, 0, 16);

            using (var rfc = new Rfc2898DeriveBytes(input, salt, 10000))
            {
                hashedInput = rfc.GetBytes(20);
            }

            for (int i = 0; i < 20; i++)
            {
                if(hashedInput[i] != hashedPassword[i]) return false;
            }

            return true;
        }
    }
}
