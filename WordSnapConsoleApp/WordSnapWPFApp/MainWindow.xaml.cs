// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp
{
    using Serilog;
    using System.Windows;
    using WordSnapWPFApp.BLL.Services;
    using WordSnapWPFApp.Presentation.Pages;

    /// <summary>
    /// main window.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.MainFrame.Navigate(new MainPage());
            this.UpdateUIForLoginState();
        }

        /// <summary>
        /// updates the UI based on whether the user is logged in or not.
        /// </summary>
        public void UpdateUIForLoginState()
        {
            if (UserService.Instance.IsUserLoggedIn)
            {
                this.LeftPanel.Visibility = Visibility.Visible;
                this.ExitButton.Visibility = Visibility.Visible;
                this.LoginButton.Visibility = Visibility.Collapsed;
                Log.Information("UI updated: user is logged.");
            }
            else
            {
                this.LeftPanel.Visibility = Visibility.Collapsed;
                this.ExitButton.Visibility = Visibility.Collapsed;
                this.LoginButton.Visibility = Visibility.Visible;
                Log.Information("UI updated: user exited.");
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new LoginPage());
            Log.Debug("Redirecting to LoginPage.");
        }

        private void LogoButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new MainPage());
            Log.Debug("Redirecting to MainPage.");
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new MainPage());
            Log.Debug("Redirecting to MainPage.");
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserService.Instance.Logout();
            this.UpdateUIForLoginState();
            this.MainFrame.Navigate(new MainPage());
            Log.Debug("Redirecting to MainPage.");
        }

        private void OwnedCardsetsLibraryButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new OwnedCardsetLibraryPage());
            Log.Debug("Redirecting to OwnedCardsetLibraryPage.");
        }

        private void AccessedCardsetLibraryButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new AccessedCardsetLibraryPage());
            Log.Debug("Redirecting to AccessedCardsetLibraryPage.");
        }
    }
}
