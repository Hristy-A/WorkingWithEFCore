using System;

namespace Store.Infrastructure.HashProviders
{
    public class BCryptHashProvider : IPasswordHashProvider
    {
        /// <summary>
        /// Generate hash with salt from password, using BCrypt provider
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hashed password.</returns>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="password"/> is null.</exception>
        public string GenerateHash(string password)
        {
            _ = password ?? throw new ArgumentNullException(nameof(password));

            var hash = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
            return hash;
        }

        /// <summary>
        /// Verifies that the hash of the given <paramref name="password"/> matches the provided <paramref name="hash"/>, using BCrypt provider
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="hash">The previously-hashed password.</param>
        /// <returns>true if the passwords match, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="password"/> or <paramref name="password"/> is null.</exception>
        public bool Verify(string password, string hash)
        {
            _ = password ?? throw new ArgumentNullException(nameof(password));
            _ = hash ?? throw new ArgumentNullException(nameof(password));

            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
        }
    }
}
