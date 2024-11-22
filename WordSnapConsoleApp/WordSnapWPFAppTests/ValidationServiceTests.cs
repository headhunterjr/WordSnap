// <copyright file="ValidationServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFAppTests
{
    using WordSnapWPFApp.BLL.Services;

    /// <summary>
    /// ValidationService tests.
    /// </summary>
    public class ValidationServiceTests
    {
        private readonly ValidationService validationService = new ValidationService();

        /// <summary>
        /// ValidateUsername should return invalid when the username is empty.
        /// </summary>
        [Fact]
        public void ValidateUsername_ReturnsInvalid_WhenUsernameIsEmpty()
        {
            // Arrange
            string username = string.Empty;

            // Act
            var result = this.validationService.ValidateUsername(username);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Ім'я користувача не може бути порожнім.", result.ErrorMessage);
        }

        /// <summary>
        /// ValidateUsername should return invalid when the username has an invalid character.
        /// </summary>
        [Fact]
        public void ValidateUsername_ReturnsInvalid_WhenUsernameHasInvalidCharacters()
        {
            // Arrange
            string username = "Invalid!";

            // Act
            var result = this.validationService.ValidateUsername(username);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Ім'я користувача може містити лише малі літери, цифри, та нижні підкреслювання.", result.ErrorMessage);
        }

        /// <summary>
        /// ValidateUsername should return valid when the username is correct.
        /// </summary>
        [Fact]
        public void ValidateUsername_ReturnsValid_WhenUsernameIsCorrect()
        {
            // Arrange
            string username = "valid_username123";

            // Act
            var result = this.validationService.ValidateUsername(username);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessage);
        }

        /// <summary>
        /// ValidateEmail should return invalid when the email is empty.
        /// </summary>
        [Fact]
        public void ValidateEmail_ReturnsInvalid_WhenEmailIsEmpty()
        {
            // Arrange
            string email = string.Empty;

            // Act
            var result = this.validationService.ValidateEmail(email);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Пошта не може бути порожньою.", result.ErrorMessage);
        }

        /// <summary>
        /// ValidateEmail should return invalid when the email is incorrect.
        /// </summary>
        [Fact]
        public void ValidateEmail_ReturnsInvalid_WhenEmailIsIncorrect()
        {
            // Arrange
            string email = "invalid-email";

            // Act
            var result = this.validationService.ValidateEmail(email);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Введіть справжню електронну адресу.", result.ErrorMessage);
        }

        /// <summary>
        /// ValidateEmail should return valid if the email is correct.
        /// </summary>
        [Fact]
        public void ValidateEmail_ReturnsValid_WhenEmailIsCorrect()
        {
            // Arrange
            string email = "example@test.com";

            // Act
            var result = this.validationService.ValidateEmail(email);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessage);
        }

        /// <summary>
        /// ValidatePassword should return invalid if the password is empty.
        /// </summary>
        [Fact]
        public void ValidatePassword_ReturnsInvalid_WhenPasswordIsEmpty()
        {
            // Arrange
            string password = string.Empty;

            // Act
            var result = this.validationService.ValidatePassword(password);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Пароль не може бути порожнім.", result.ErrorMessage);
        }

        /// <summary>
        /// ValidatePassword should return invalid if the password is too short.
        /// </summary>
        [Fact]
        public void ValidatePassword_ReturnsInvalid_WhenPasswordIsTooShort()
        {
            // Arrange
            string password = "Short1";

            // Act
            var result = this.validationService.ValidatePassword(password);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Пароль повинен складатися принаймні з 8 символів.", result.ErrorMessage);
        }

        /// <summary>
        /// ValidatePassword should return invalid if the password lacks uppercase letters.
        /// </summary>
        [Fact]
        public void ValidatePassword_ReturnsInvalid_WhenPasswordLacksUpperCase()
        {
            // Arrange
            string password = "password1";

            // Act
            var result = this.validationService.ValidatePassword(password);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Пароль повинен містити принаймні одну велику літеру.", result.ErrorMessage);
        }

        /// <summary>
        /// ValidatePassword should return invalid if the password lacks lowercase letters.
        /// </summary>
        [Fact]
        public void ValidatePassword_ReturnsInvalid_WhenPasswordLacksLowerCase()
        {
            // Arrange
            string password = "PASSWORD1";

            // Act
            var result = this.validationService.ValidatePassword(password);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Пароль повинен містити принаймні одну малу літеру.", result.ErrorMessage);
        }

        /// <summary>
        /// ValidatePassword should return invalid if the password lacks digits.
        /// </summary>
        [Fact]
        public void ValidatePassword_ReturnsInvalid_WhenPasswordLacksDigits()
        {
            // Arrange
            string password = "Password";

            // Act
            var result = this.validationService.ValidatePassword(password);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Пароль повинен містити принаймні одну цифру.", result.ErrorMessage);
        }

        /// <summary>
        /// ValidatePassword should return valid if the password is correct.
        /// </summary>
        [Fact]
        public void ValidatePassword_ReturnsValid_WhenPasswordIsCorrect()
        {
            // Arrange
            string password = "Password1";

            // Act
            var result = this.validationService.ValidatePassword(password);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessage);
        }

        /// <summary>
        /// ValidateEnglishText should return invalid if the text is empty.
        /// </summary>
        [Fact]
        public void ValidateEnglishText_ReturnsInvalid_WhenTextIsEmpty()
        {
            // Arrange
            string text = string.Empty;

            // Act
            var result = this.validationService.ValidateEnglishText(text);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Текст не може бути порожнім.", result.ErrorMessage);
        }

        /// <summary>
        /// ValidateEnglishText should return invalid if the text is too long.
        /// </summary>
        [Fact]
        public void ValidateEnglishText_ReturnsInvalid_WhenTextExceedsMaxLength()
        {
            // Arrange
            string text = new string('a', 101);

            // Act
            var result = this.validationService.ValidateEnglishText(text);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Текст не може перевищувати 100 символів.", result.ErrorMessage);
        }

        /// <summary>
        /// ValidateEnglishText should return valid if the text is correct.
        /// </summary>
        [Fact]
        public void ValidateEnglishText_ReturnsValid_WhenTextIsCorrect()
        {
            // Arrange
            string text = "Valid text";

            // Act
            var result = this.validationService.ValidateEnglishText(text);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessage);
        }

        /// <summary>
        /// ValidateEnglishText should return invalid if it contains invalid characters.
        /// </summary>
        [Fact]
        public void ValidateEnglishText_ReturnsInvalid_WhenInvalidCharactersUsed()
        {
            // Arrange
            string text = "Invalid@text!";

            // Act
            var result = this.validationService.ValidateEnglishText(text, withNumbersAndUnderscores: true);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Текст може містити лише англійські літери та пробіли.", result.ErrorMessage);
        }

        /// <summary>
        /// ValidateUkrainianText should return invalid if the text is empty.
        /// </summary>
        [Fact]
        public void ValidateUkrainianText_ReturnsInvalid_WhenTextIsEmpty()
        {
                // Arrange
                string text = string.Empty;

                // Act
                var result = this.validationService.ValidateUkrainianText(text);

                // Assert
                Assert.False(result.IsValid);
                Assert.Equal("Текст не може бути порожнім.", result.ErrorMessage);
        }

        /// <summary>
        /// ValidateUkrainianText should return invalid if the text is too long.
        /// </summary>
        [Fact]
        public void ValidateUkrainianText_ReturnsInvalid_WhenTextExceedsMaxLength()
        {
                // Arrange
                string text = new string('а', 101);

                // Act
                var result = this.validationService.ValidateUkrainianText(text);

                // Assert
                Assert.False(result.IsValid);
                Assert.Equal("Текст не може перевищувати 100 символів.", result.ErrorMessage);
        }

        /// <summary>
        /// ValidateUkrainianText should return valid if the text is correct.
        /// </summary>
        [Fact]
        public void ValidateUkrainianText_ReturnsValid_WhenTextIsCorrect()
        {
                // Arrange
                string text = "Текст українською мовою";

                // Act
                var result = this.validationService.ValidateUkrainianText(text);

                // Assert
                Assert.True(result.IsValid);
                Assert.Empty(result.ErrorMessage);
        }

        /// <summary>
        /// ValidateUkrainianText should return valid if the text is correct with the "numbers and underscores" option enabled.
        /// </summary>
        [Fact]
        public void ValidateUkrainianText_ReturnsValid_WhenTextIncludesNumbersAndUnderscores_WithOptionEnabled()
        {
                // Arrange
                string text = "Текст 123_з цифрами";

                // Act
                var result = this.validationService.ValidateUkrainianText(text, withNumbersAndUnderscores: true);

                // Assert
                Assert.True(result.IsValid);
                Assert.Empty(result.ErrorMessage);
        }

        /// <summary>
        /// ValidateUkrainianText should return valid if the text contains apostrophes.
        /// </summary>
        [Fact]
        public void ValidateUkrainianText_ReturnsValid_WhenTextContainsApostrophes()
        {
                // Arrange
                string text = "Текст з апострофом'";

                // Act
                var result = this.validationService.ValidateUkrainianText(text);

                // Assert
                Assert.True(result.IsValid);
                Assert.Empty(result.ErrorMessage);
        }
    }
}
