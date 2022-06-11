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
        public string GenerateHash(string password) =>
            password is null
                ? throw new ArgumentNullException(nameof(password))
                : BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        /// <summary>
        /// Verifies that the hash of the given <paramref name="password"/> matches the provided <paramref name="hash"/>, using BCrypt provider
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="hash">The previously-hashed password.</param>
        /// <returns>true if the passwords match, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="password"/> or <paramref name="password"/> is null.</exception>
        public bool Verify(string password, string hash) =>
            password is null || hash is null
                ? throw new ArgumentNullException(nameof(password))
                : BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}
