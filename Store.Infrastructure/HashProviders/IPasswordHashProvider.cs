namespace Store.Infrastructure.HashProviders
{
    public interface IPasswordHashProvider
    {
        string GenerateHash(string password);
        bool Verify(string password, string hash);
    }
}