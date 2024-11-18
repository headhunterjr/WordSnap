// <copyright file="ValidationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.BLL.Services
{
    using System.Text.RegularExpressions;
    using System.Windows;

    /// <summary>
    /// validation service.
    /// </summary>
    public class ValidationService
    {
        /// <summary>
        /// validates a username.
        /// </summary>
        /// <param name="username">username.</param>
        /// <returns>a validation result.</returns>
        public ValidationResult ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return new ValidationResult(false, "Ім'я користувача не може бути порожнім.");
            }

            var regex = new Regex("^[a-z0-9_]+$");

            if (!regex.IsMatch(username))
            {
                return new ValidationResult(false, "Ім'я користувача може містити лише малі літери, цифри, та нижні підкреслювання.");
            }

            return new ValidationResult(true);
        }

        /// <summary>
        /// validates an email.
        /// </summary>
        /// <param name="email">email.</param>
        /// <returns>a validation result.</returns>
        public ValidationResult ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new ValidationResult(false, "Пошта не може бути порожньою.");
            }

            var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

            if (!regex.IsMatch(email))
            {
                return new ValidationResult(false, "Введіть справжню електронну адресу.");
            }

            return new ValidationResult(true);
        }

        /// <summary>
        /// validates a password.
        /// </summary>
        /// <param name="password">password.</param>
        /// <returns>a validation result.</returns>
        public ValidationResult ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return new ValidationResult(false, "Пароль не може бути порожнім.");
            }

            if (password.Length < 8)
            {
                return new ValidationResult(false, "Пароль повинен складатися принаймні з 8 символів.");
            }

            if (!password.Any(char.IsUpper))
            {
                return new ValidationResult(false, "Пароль повинен містити принаймні одну велику літеру.");
            }

            if (!password.Any(char.IsLower))
            {
                return new ValidationResult(false, "Пароль повинен містити принаймні одну малу літеру.");
            }

            if (!password.Any(char.IsDigit))
            {
                return new ValidationResult(false, "Пароль повинен містити принаймні одну цифру.");
            }

            return new ValidationResult(true);
        }

        /// <summary>
        /// validates a registration form.
        /// </summary>
        /// <param name="username">username.</param>
        /// <param name="email">email.</param>
        /// <param name="password">password.</param>
        /// <returns>a bool indicating whether the form is valid.</returns>
        public bool ValidateRegistrationForm(string username, string email, string password)
        {
            bool isValid = true;

            var usernameValidation = this.ValidateUsername(username);
            if (!usernameValidation.IsValid)
            {
                MessageBox.Show(usernameValidation.ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                isValid = false;
            }

            var emailValidation = this.ValidateEmail(email);
            if (!emailValidation.IsValid)
            {
                MessageBox.Show(emailValidation.ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                isValid = false;
            }

            var passwordValidation = this.ValidatePassword(password);
            if (!passwordValidation.IsValid)
            {
                MessageBox.Show(passwordValidation.ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// validates english text.
        /// </summary>
        /// <param name="text">text.</param>
        /// <param name="withNumbersAndUnderscores">boolean value indicating whether numbers and underscores can be included.</param>
        /// <returns>a validation result.</returns>
        public ValidationResult ValidateEnglishText(string text, bool withNumbersAndUnderscores = false)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return new ValidationResult(false, "Текст не може бути порожнім.");
            }

            if (text.Length > 100)
            {
                return new ValidationResult(false, "Текст не може перевищувати 100 символів.");
            }

            Regex regex;
            if (withNumbersAndUnderscores)
            {
                regex = new Regex("^[a-zA-Z0-9_ ]+$");
            }
            else
            {
                regex = new Regex("^[a-zA-Z ]+$");
            }

            if (!regex.IsMatch(text))
            {
                return new ValidationResult(false, withNumbersAndUnderscores ? "Текст може містити лише англійські літери та пробіли." : "Текст може містити лише англійські літери, пробіли, цифри та підкреслювання.");
            }

            return new ValidationResult(true);
        }

        /// <summary>
        /// validates ukrainian text.
        /// </summary>
        /// <param name="text">text.</param>
        /// <param name="withNumbersAndUnderscores">a boolean value indicating whether numbers and underscores can be included.</param>
        /// <returns>a validation result.</returns>
        public ValidationResult ValidateUkrainianText(string text, bool withNumbersAndUnderscores = false)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return new ValidationResult(false, "Текст не може бути порожнім.");
            }

            if (text.Length > 100)
            {
                return new ValidationResult(false, "Текст не може перевищувати 100 символів.");
            }

            Regex regex;
            if (withNumbersAndUnderscores)
            {
                regex = new Regex("^[а-щА-ЩЬьЮюЯяЇїІіЄєҐґ'0-9_ ]+$");
            }
            else
            {
                regex = new Regex("^[а-щА-ЩЬьЮюЯяЇїІіЄєҐґ' ]+$");
            }

            if (!regex.IsMatch(text))
            {
                return new ValidationResult(false, withNumbersAndUnderscores ? "Текст може містити лише українські літери та пробіли." : "Текст може містити лише українські літери, пробіли, цифри та підкреслювання.");
            }

            return new ValidationResult(true);
        }

        /// <summary>
        /// validation result.
        /// </summary>
        public class ValidationResult
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ValidationResult"/> class.
            /// </summary>
            /// <param name="isValid">checks if validation passed.</param>
            /// <param name="errorMessage">error message.</param>
            public ValidationResult(bool isValid, string errorMessage = "")
            {
                this.IsValid = isValid;
                this.ErrorMessage = errorMessage;
            }

            /// <summary>
            /// Gets or sets a value indicating whether a validation was valid.
            /// </summary>
            public bool IsValid { get; set; }

            /// <summary>
            /// Gets or sets an arror message.
            /// </summary>
            public string ErrorMessage { get; set; }
        }
    }
}
