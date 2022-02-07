namespace Store.Infrastructure.HashProviders
{
    public class BCryptHashProvider : IPasswordHashProvider
    {
        public string GenerateHash(string input)
        {
            var hash = BCrypt.Net.BCrypt.EnhancedHashPassword(input);
            return hash;
        }

        public bool Verify(string input, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(input, hash);
        }
    }
}
