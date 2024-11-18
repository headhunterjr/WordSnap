// <copyright file="RegistrationPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.Presentation.Pages
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using WordSnapWPFApp.BLL.Services;

    /// <summary>
    /// Interaction logic for RegistrationPage.xaml.
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private readonly ValidationService validationService = new ValidationService();

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationPage"/> class.
        /// </summary>
        public RegistrationPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets username.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        public string? Password { get; set; }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            if (!this.validationService.ValidateRegistrationForm(this.Username, this.Email, this.Password))
            {
                return;
            }

            try
            {
                await UserService.Instance.RegisterUserAsync(this.Username, this.Email, this.Password);
                this.NavigationService.Navigate(new LoginPage());
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при реєстрації: " + ex.Message);
            }
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                this.Password = passwordBox.Password;
            }
        }
    }
}
