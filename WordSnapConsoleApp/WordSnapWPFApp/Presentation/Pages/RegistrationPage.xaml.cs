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
        private readonly ValidationService _validationService = new ValidationService();
        public string? Username {  get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            if (!_validationService.ValidateRegistrationForm(Username, Email, Password))
            {
                return;
            }
            try
            {
                await UserService.Instance.RegisterUserAsync(Username, Email, Password);
                NavigationService.Navigate(new LoginPage());
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
                Password = passwordBox.Password;
            }
        }
    }
}
