using System;
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
        //TODO: тест на юникодные символы + если код свалится, то подумать, как быть
        public void GenerateHashAndLogging(string password)
        {
            VanillaHashProvider provider = new VanillaHashProvider();
            string hash = provider.GenerateHash(password);

            Assert.NotNull(hash);
            Assert.True(provider.Verify(password, hash));
        }

        [Fact]
        public void GenerateHash_Fails_WhenInputIsNull()
        {
            // Arrange
            IPasswordHashProvider provider = new VanillaHashProvider();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                provider.GenerateHash(null);
            });
        }
    }
}
