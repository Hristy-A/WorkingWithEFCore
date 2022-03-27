using Store.Infrastructure.HashProviders;
using System;
using Xunit;

namespace UnitTests.HashProvidersTests
{
    public class VanillaHashProviderTests
    {
        private static IPasswordHashProvider _passwordHashProvider = new VanillaHashProvider();

        #region GenerateHashTests
        [Theory]
        [InlineData("")]
        [InlineData("qwerty")]
        [InlineData("⚒⚓☣✈")]
        [InlineData("\\\n\t\r\f\b")]
        public void GenerateHash_WithDifferenceNotNullInput_SuccesfullGenerateHash(string password)
        {
            // Arrange
            string hash;

            // Act
            hash = _passwordHashProvider.GenerateHash(password);

            // Assert
            Assert.NotNull(hash);
            Assert.NotEmpty(hash);
        }

        [Theory]
        [InlineData("")]
        [InlineData("password")]
        [InlineData("\\\n\t\r\f\b")]
        public void GenerateHash_WithTheSameInput_ReturnDifferentHashes(string password)
        {
            // Arrange
            string hash1, hash2;

            // Act
            hash1 = _passwordHashProvider.GenerateHash(password);
            hash2 = _passwordHashProvider.GenerateHash(password);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void GenerateHash_WhenInputIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _passwordHashProvider.GenerateHash(null);
            });
        }
        #endregion

        #region VerifyTests
        [Theory]
        [InlineData("correctPassword", null)]
        [InlineData(null, "_&&Fksejf325jfFjei2425jgt2otj23jTJ")]
        [InlineData(null, null)]
        public void Verify_WhenOneOfTheOrBothInputsNull_ShouldThrowArgumentNullException(string password, string hash)
        {
            // Arrange
            Action action = () => _passwordHashProvider.Verify(password, hash);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t\\\n")]
        [InlineData("⚒⚓☣✈")]
        [InlineData("randomPassword")]
        public void Verify_WithIncorrectPassword_ReturnFalse(string password)
        {
            // Arrange
            string incorrectPassword = "incorrectPassword";
            string hash;
            bool result;

            // Act
            hash = _passwordHashProvider.GenerateHash(password);
            result = _passwordHashProvider.Verify(incorrectPassword, hash);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t\\\n")]
        [InlineData("⚒⚓☣✈")]
        [InlineData("randomPassword")]
        public void Verify_WithCorrectPassword_ReturnTrue(string password)
        {
            // Arrange
            string correctPassword = password;
            string hash;
            bool result;

            // Act
            hash = _passwordHashProvider.GenerateHash(password);
            result = _passwordHashProvider.Verify(correctPassword, hash);

            // Assert
            Assert.True(result);
        }
        #endregion
    }
}
