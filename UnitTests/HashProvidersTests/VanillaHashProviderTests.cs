using Store.Infrastructure.HashProviders;
using Xunit;

namespace UnitTests.HashProvidersTests
{
    public class VanillaHashProviderTests
    {
        [Theory]
        [InlineData("qwerty")]
        [InlineData("12345678")]
        [InlineData("ABCDEFG")]
        public void GenerateHashAndLogging(string password)
        {
            VanillaHashProvider provider = new VanillaHashProvider();
            string hash = provider.GenerateHash(password);

            Assert.NotNull(hash);
            Assert.True(provider.Verify(password, hash));
        }
    }
}
