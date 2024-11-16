using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace WordSnapWPFApp.BLL.Services
{
    public class ValidationService
    {
        public class ValidationResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }

            public ValidationResult(bool isValid, string errorMessage = "")
            {
                IsValid = isValid;
                ErrorMessage = errorMessage;
            }
        }

        public ValidationResult ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return new ValidationResult(false, "Ім'я користувача не може бути порожнім.");

            var regex = new Regex("^[a-z0-9_]+$");

            if (!regex.IsMatch(username))
                return new ValidationResult(false, "Ім'я користувача може містити лише малі літери, цифри, та нижні підкреслювання.");

            return new ValidationResult(true);
        }

        public ValidationResult ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return new ValidationResult(false, "Пошта не може бути порожньою.");

            var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

            if (!regex.IsMatch(email))
                return new ValidationResult(false, "Введіть справжню електронну адресу.");

            return new ValidationResult(true);
        }

        public ValidationResult ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return new ValidationResult(false, "Пароль не може бути порожнім.");

            if (password.Length < 8)
                return new ValidationResult(false, "Пароль повинен складатися принаймні з 8 символів.");

            if (!password.Any(char.IsUpper))
                return new ValidationResult(false, "Пароль повинен містити принаймні одну велику літеру.");

            if (!password.Any(char.IsLower))
                return new ValidationResult(false, "Пароль повинен містити принаймні одну малу літеру.");

            if (!password.Any(char.IsDigit))
                return new ValidationResult(false, "Пароль повинен містити принаймні одну цифру.");

            return new ValidationResult(true);
        }

        public bool ValidateRegistrationForm(string username, string email, string password)
        {
            bool isValid = true;

            var usernameValidation = ValidateUsername(username);
            if (!usernameValidation.IsValid)
            {
                MessageBox.Show(usernameValidation.ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                isValid = false;
            }

            var emailValidation = ValidateEmail(email);
            if (!emailValidation.IsValid)
            {
                MessageBox.Show(emailValidation.ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                isValid = false;
            }

            var passwordValidation = ValidatePassword(password);
            if (!passwordValidation.IsValid)
            {
                MessageBox.Show(passwordValidation.ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                isValid = false;
            }

            return isValid;
        }
        public ValidationResult ValidateEnglishText(string text, bool withNumbersAndUnderscores = false)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new ValidationResult(false, "Текст не може бути порожнім.");

            if (text.Length > 100)
                return new ValidationResult(false, "Текст не може перевищувати 100 символів.");

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
                return new ValidationResult(false, withNumbersAndUnderscores ? "Текст може містити лише англійські літери та пробіли." 
                    : "Текст може містити лише англійські літери, пробіли, цифри та підкреслювання.");

            return new ValidationResult(true);
        }

        public ValidationResult ValidateUkrainianText(string text, bool withNumbersAndUnderscores = false)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new ValidationResult(false, "Текст не може бути порожнім.");

            if (text.Length > 100)
                return new ValidationResult(false, "Текст не може перевищувати 100 символів.");

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
                return new ValidationResult(false, withNumbersAndUnderscores ? "Текст може містити лише українські літери та пробіли."
                    : "Текст може містити лише українські літери, пробіли, цифри та підкреслювання.");

            return new ValidationResult(true);
        }

    }
}
