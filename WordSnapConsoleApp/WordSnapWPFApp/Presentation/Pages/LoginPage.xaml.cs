// <copyright file="LoginPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.Presentation.Pages
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using WordSnapWPFApp.BLL.Services;

    /// <summary>
    /// Interaction logic for LoginPage.xaml.
    /// </summary>
    public partial class LoginPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage"/> class.
        /// </summary>
        public LoginPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        public string? Password { get; set; }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegistrationPage());
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(this.Password))
            {
                MessageBox.Show("Введіть пошту та пароль.");
                return;
            }

            try
            {
                var user = await UserService.Instance.LoginUserAsync(this.Email, this.Password);
                if (user != null)
                {
                    if (Application.Current.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.UpdateUIForLoginState();
                    }

                    this.NavigationService.Navigate(new MainPage());
                }
                else
                {
                    MessageBox.Show("Неправильна пошта або пароль.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка під час входу: " + ex.Message);
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
