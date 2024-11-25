// <copyright file="OwnedCardsetLibraryPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.Presentation.Pages
{
    using System.Windows;
    using System.Windows.Controls;
    using Serilog;
    using WordSnapWPFApp.BLL.Services;
    using WordSnapWPFApp.DAL.Models;

    /// <summary>
    /// Interaction logic for OwnedCardsetLibraryPage.xaml.
    /// </summary>
    public partial class OwnedCardsetLibraryPage : Page
    {
        private readonly CardsetService cardsetService = new CardsetService();

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnedCardsetLibraryPage"/> class.
        /// </summary>
        public OwnedCardsetLibraryPage()
        {
            this.InitializeComponent();
            this.InitializeCardsets();
            Log.Information("OwnedCardsetLibraryPage initialized.");
        }

        private async void InitializeCardsets()
        {
            int userId = UserService.Instance.GetLoggedInUser().Id;
            var cardsets = await this.cardsetService.GetUsersOwnCardsetsLibraryAsync(userId);
            this.CardsetsListBox.ItemsSource = cardsets;
            Log.Information("Cardsets initialized.");
        }

        private void CardsetButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Cardset cardset)
            {
                this.NavigationService.Navigate(new CardsetPage(cardset.Id, cardset.Name));
                Log.Debug("Redirecting to CardsetPage.");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EditOrCreateCardsetPage());
            Log.Debug("Redirecting to EditOrCreateCardsetPage.");
        }
    }
}
