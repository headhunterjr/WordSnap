using WordSnapWPFApp.BLL.Services;

namespace WordSnapWPFAppTests
{
    public class PasswordServiceTests
    {
        [Fact]
        public void GenerateSalt_ShouldReturnUniqueValues()
        {
            // Act
            var salt1 = PasswordService.GenerateSalt();
            var salt2 = PasswordService.GenerateSalt();

            // Assert
            Assert.NotNull(salt1);
            Assert.NotNull(salt2);
            Assert.NotEqual(salt1, salt2);
        }

        [Fact]
        public void HashPassword_ShouldReturnConsistentHashForSameInputs()
        {
            // Arrange
            var password = "MySecurePassword";
            var salt = PasswordService.GenerateSalt();

            // Act
            var hash1 = PasswordService.HashPassword(password, salt);
            var hash2 = PasswordService.HashPassword(password, salt);

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void HashPassword_ShouldReturnDifferentHashesForDifferentSalts()
        {
            // Arrange
            var password = "MySecurePassword";
            var salt1 = PasswordService.GenerateSalt();
            var salt2 = PasswordService.GenerateSalt();

            // Act
            var hash1 = PasswordService.HashPassword(password, salt1);
            var hash2 = PasswordService.HashPassword(password, salt2);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void HashPassword_ShouldReturnDifferentHashesForDifferentPasswords()
        {
            // Arrange
            var password1 = "Password1";
            var password2 = "Password2";
            var salt = PasswordService.GenerateSalt();

            // Act
            var hash1 = PasswordService.HashPassword(password1, salt);
            var hash2 = PasswordService.HashPassword(password2, salt);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}