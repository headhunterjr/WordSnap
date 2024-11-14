using System.Windows;
using System.Windows.Controls;
using WordSnapWPFApp.BLL.Services;
using WordSnapWPFApp.Presentation.Pages;

namespace WordSnapWPFApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MainPage());
            UpdateUIForLoginState();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new LoginPage());
        }

        private void LogoButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainPage());
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainPage());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserService.Instance.Logout();
            UpdateUIForLoginState();
            MainFrame.Navigate(new MainPage());
        }

        public void UpdateUIForLoginState()
        {
            if (UserService.Instance.IsUserLoggedIn)
            {
                LeftPanel.Visibility = Visibility.Visible;
                LoginButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                LeftPanel.Visibility = Visibility.Collapsed;
                LoginButton.Visibility = Visibility.Visible;
            }
        }

        private void OwnedCardsetsLibraryButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OwnedCardsetLibraryPage());
        }
    }
}
