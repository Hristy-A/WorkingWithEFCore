namespace Store.Infrastructure.HashProviders
{
    public interface IPasswordHashProvider
    {
        string GenerateHash(string passwor);
        bool Verify(string password, string hash);
    }
}