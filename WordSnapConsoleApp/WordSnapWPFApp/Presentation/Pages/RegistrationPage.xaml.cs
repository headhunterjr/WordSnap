using System;
using System.Collections.Generic;
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
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private readonly UserService _userService = new UserService();
        public string? Username {  get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Email)|| string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Please fill out all the fields");
                return;
            }
            try
            {
                await _userService.RegisterUserAsync(Username, Email, Password);
                NavigationService.Navigate(new LoginPage());
            }
            catch (InvalidOperationException ex) 
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during registration: " + ex.Message + (ex.InnerException != null ? " | Inner Exception: " + ex.InnerException.Message : ""));
            }
        }
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                Password = passwordBox.Password;
            }
        }
        public void Dispose()
        {
            _userService?.Dispose();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }
    }
}
