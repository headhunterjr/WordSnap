// <copyright file="AccessedCardsetLibraryPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.Presentation.Pages
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using WordSnapWPFApp.BLL.Services;
    using WordSnapWPFApp.DAL.Models;

    /// <summary>
    /// Interaction logic for AccessedCardsetLibraryPage.xaml.
    /// </summary>
    public partial class AccessedCardsetLibraryPage : Page
    {
        private readonly CardsetService cardsetService = new CardsetService();

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessedCardsetLibraryPage"/> class.
        /// </summary>
        public AccessedCardsetLibraryPage()
        {
            this.InitializeComponent();
            this.InitializeCardsets();
        }

        private async void InitializeCardsets()
        {
            int userId = UserService.Instance.GetLoggedInUser().Id;
            var cardsets = await this.cardsetService.GetUsersCardsetsLibraryAsync(userId);
            this.CardsetsListBox.ItemsSource = cardsets;
        }

        private void CardsetButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Cardset cardset)
            {
                this.NavigationService.Navigate(new CardsetPage(cardset.Id, cardset.Name));
            }
        }
    }
}
