namespace Store.Infrastructure.HashProviders
{
    public interface IPasswordHashProvider
    {
        string GenerateHash(string input);
        bool Verify(string input, string hash);
    }
}
