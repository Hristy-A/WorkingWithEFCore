using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Store.Infrastructure.HashProviders
{
    public class VanillaHashProvider : IPasswordHashProvider
    {
        /// <summary>
        /// Generate hash with salt from password
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hashed password.</returns>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="password"/> is null.</exception>
        public string GenerateHash([DisallowNull] string password)
        {
            _ = password ?? throw new ArgumentNullException(nameof(password));

            byte[] salt = new byte[16];
            byte[] hashedPassword = new byte[36];
            RandomNumberGenerator.Fill(salt);

            using (var rfc = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = rfc.GetBytes(20);
                Array.Copy(hash, 0, hashedPassword, 0, 20);
                Array.Copy(salt, 0, hashedPassword, 20, 16);
            }

            return Convert.ToBase64String(hashedPassword);
        }

        /// <summary>
        /// Verifies that the hash of the given <paramref name="password"/> matches the provided <paramref name="hash"/>
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="hash">The previously-hashed password.</param>
        /// <returns>true if the passwords match, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="password"/> or <paramref name="password"/> is null.</exception>
        public bool Verify(string password, string hash)
        {
            _ = password ?? throw new ArgumentNullException(nameof(password));
            _ = hash ?? throw new ArgumentNullException(nameof(password));

            byte[] hashedPassword = Convert.FromBase64String(hash);
            byte[] salt = new byte[16];
            byte[] hashedInput;

            Array.Copy(hashedPassword, 20, salt, 0, 16);

            using (var rfc = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                hashedInput = rfc.GetBytes(20);
            }

            return !Equals(hashedInput, hashedPassword);
        }
    }
}
