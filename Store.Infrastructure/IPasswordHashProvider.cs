namespace Store.Infrastructure
{
    public interface IPasswordHashProvider
    {
        string GenerateHash(string input);
        bool Verify(string input, string hash);
    }
}
