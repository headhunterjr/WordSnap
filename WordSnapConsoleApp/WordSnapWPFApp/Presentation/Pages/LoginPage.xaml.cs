using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordSnapWPFApp.BLL.Services;
using WordSnapWPFApp.DAL.Models;

namespace WordSnapWPFApp.Presentation.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public string? Email {  get; set; }
        public string? Password { get; set; }
        public LoginPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Введіть пошту та пароль.");
                return;
            }
            try
            {
                var user = await UserService.Instance.LoginUserAsync(Email, Password);
                if (user != null)
                {
                    if (Application.Current.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.UpdateUIForLoginState();
                    }
                    NavigationService.Navigate(new MainPage());
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
                Password = passwordBox.Password;
            }
        }
    }
}
