using Store.Infrastructure.HashProviders;
using System;
using Xunit;

namespace UnitTests.HashProvidersTests
{
    //TODO: проверить имена тестов, убрать избыточные Arrange секции
    public class BCryptHashProviderTests
    {
        private static IPasswordHashProvider _passwordHashProvider = new BCryptHashProvider();

        #region GenerateHashTests
        [Theory]
        [InlineData("")]
        [InlineData("qwerty")]
        [InlineData("⚒⚓☣✈")]
        [InlineData("\\\n\t\r\f\b")]
        public void GenerateHash_WithNotNullInput_SuccesfullGenerateHash(string password)
        {
            // Act
            string hash = _passwordHashProvider.GenerateHash(password);

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
            // Act
            string hash1 = _passwordHashProvider.GenerateHash(password);
            string hash2 = _passwordHashProvider.GenerateHash(password);

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
            string correctPasswordHash = _passwordHashProvider.GenerateHash(password);
            string incorrectPassword = "incorrectPassword";

            // Act
            bool isPasswordValid = _passwordHashProvider.Verify(incorrectPassword, correctPasswordHash);

            // Assert
            Assert.False(isPasswordValid);
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
            string passwordHash = _passwordHashProvider.GenerateHash(password);

            // Act
            bool isPasswordValid = _passwordHashProvider.Verify(password, passwordHash);

            // Assert
            Assert.True(isPasswordValid);
        }
        #endregion
    }
}
