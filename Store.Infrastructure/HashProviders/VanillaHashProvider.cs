using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Store.Infrastructure.HashProviders
{
    public class VanillaHashProvider : IPasswordHashProvider
    {
        // Attention! Don't change this constats, if your database
        // alredy contains password, hashed by this algorithm.
        const int HashLength = 20;
        const int SaltLength = 16;
        const int NumberOfHashingIterations = 100000;

        /// <summary>
        /// Generate hash with salt from password
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hashed password.</returns>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="password"/> is null.</exception>
        public string GenerateHash([DisallowNull] string password)
        {
            _ = password ?? throw new ArgumentNullException(nameof(password));

            int totalHashLength = SaltLength + HashLength;
            byte[] salt = new byte[SaltLength];
            byte[] hashedPasswordWithSalt = new byte[totalHashLength];
            RandomNumberGenerator.Fill(salt);

            using (var rfc = new Rfc2898DeriveBytes(password, salt, NumberOfHashingIterations))
            {
                byte[] hash = rfc.GetBytes(HashLength);
                int saltStartIndex = HashLength;
                Array.Copy(hash, 0, hashedPasswordWithSalt, 0, HashLength);
                Array.Copy(salt, 0, hashedPasswordWithSalt, saltStartIndex, SaltLength);
            }

            return Convert.ToBase64String(hashedPasswordWithSalt);
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

            byte[] hashedPasswordWithSalt = Convert.FromBase64String(hash);
            byte[] salt = new byte[SaltLength];

            int saltStartIndex = HashLength;
            Array.Copy(hashedPasswordWithSalt, saltStartIndex, salt, 0, SaltLength);

            byte[] hashedVerifiablePassword;

            using (var rfc = new Rfc2898DeriveBytes(password, salt, NumberOfHashingIterations))
            {
                hashedVerifiablePassword = rfc.GetBytes(HashLength);
            }

            // checking only hash part of array (20 first bytes)
            for (int i = 0; i < HashLength; i++)
                if (hashedPasswordWithSalt[i] != hashedVerifiablePassword[i]) return false;
            return true;
        }
    }
}
