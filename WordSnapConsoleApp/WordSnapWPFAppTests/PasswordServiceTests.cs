// <copyright file="PasswordServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFAppTests
{
    using WordSnapWPFApp.BLL.Services;

    /// <summary>
    /// Password service tests.
    /// </summary>
    public class PasswordServiceTests
    {
        /// <summary>
        /// Generated salts should be unique.
        /// </summary>
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

        /// <summary>
        /// Hashed passwords should be the same for the same input.
        /// </summary>
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

        /// <summary>
        /// Hashed passwords should be different for different salts.
        /// </summary>
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

        /// <summary>
        /// Hashed passwords should be different for different passwords.
        /// </summary>
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