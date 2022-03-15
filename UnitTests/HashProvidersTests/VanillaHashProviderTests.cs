using Store.Infrastructure.HashProviders;
using System;
using Xunit;

namespace UnitTests.HashProvidersTests
{
    public class VanillaHashProviderTests
    {
        [Theory]
        [InlineData("qwerty")]
        [InlineData("")]
        [InlineData("⚒⚓☣✈")]
        public void GenerateHashAndLogging_WithDifferenceNotNullInput_SuccesfullGenerateHashAndLogin(string password)
        {
            // Arrange
            IPasswordHashProvider provider = new VanillaHashProvider();

            string hash;
            bool result;

            // Act
            hash = provider.GenerateHash(password);
            result = provider.Verify(password, hash);

            // Assert
            Assert.NotNull(hash);
            Assert.True(result);
        }

        [Fact]
        public void GenerateHash_WithTheSameInput_DifferentHashes()
        {
            // Arrange
            IPasswordHashProvider provider = new VanillaHashProvider();
            string password = "password";
            string hash1, hash2;

            // Act
            hash1 = provider.GenerateHash(password);
            hash2 = provider.GenerateHash(password);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void GenerateHash_WhenInputIsNull_Fail()
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

        [Fact]
        public void ValidatePassword_WhenHashIsNull_Fail()
        {
            //Arrange
            IPasswordHashProvider provider = new VanillaHashProvider();

            string password = "correctPassword";
            string hash = null;
            Action action = () => provider.Verify(password, hash);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void ValidatePassword_WhenPasswordIsNull_Fail()
        {
            //Arrange
            IPasswordHashProvider provider = new VanillaHashProvider();

            string password = null;
            string hash = "gwgjk4j1j4h1lh53k1jlh5";
            Action action = () => provider.Verify(password, hash);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void ValidatePassword_WhenAllInputsIsNull_Fail()
        {
            //Arrange
            IPasswordHashProvider provider = new VanillaHashProvider();

            string password = null;
            string hash = null;
            Action action = () => provider.Verify(password, hash);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }
    }
}
