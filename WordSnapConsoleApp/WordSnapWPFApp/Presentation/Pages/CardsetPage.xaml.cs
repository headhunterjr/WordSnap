// <copyright file="CardsetPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.Presentation.Pages
{
    using Serilog;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Animation;
    using System.Windows.Media;
    using WordSnapWPFApp.BLL.Services;
    using WordSnapWPFApp.DAL.Models;

    /// <summary>
    /// Interaction logic for CardsetPage.xaml.
    /// </summary>
    public partial class CardsetPage : Page
    {
        private readonly CardsetService cardsetService = new CardsetService();
        private int cardsetId;
        private string cardsetName;
        private Card selectedCard;
        private bool isCardsetInSavedCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardsetPage"/> class.
        /// </summary>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <param name="cardsetName">cardset's name.</param>
        public CardsetPage(int cardsetId, string cardsetName)
        {
            this.InitializeComponent();
            this.cardsetId = cardsetId;
            this.cardsetName = cardsetName;
            this.InitializeCards();
            this.UpdateUIForLoginState();
            Log.Information("CardsetPage initialized");
        }

        /// <summary>
        /// updates UI based on whether the user is logged in or not.
        /// </summary>
        public void UpdateUIForLoginState()
        {
            if (UserService.Instance.IsUserLoggedIn)
            {
                this.DeleteCardsetButton.Visibility = Visibility.Visible;
                this.ActionButton.Visibility = Visibility.Visible;
                this.EditCardsetButton.Visibility = Visibility.Visible;
                this.AddCardsetToCollectionButton.Visibility = Visibility.Visible;
                Log.Information("UI updated: user is logged.");
            }
            else
            {
                this.DeleteCardsetButton.Visibility = Visibility.Hidden;
                this.ActionButton.Visibility = Visibility.Hidden;
                this.EditCardsetButton.Visibility = Visibility.Hidden;
                this.AddCardsetToCollectionButton.Visibility = Visibility.Hidden;
                Log.Information("UI updated: user is not logged.");
            }
        }

        private async void InitializeCards()
        {
            var cards = await this.cardsetService.GetCardsOfCardsetAsync(this.cardsetId);
            this.CardsListBox.ItemsSource = cards;
            this.CardsetName.Text = this.cardsetName;

            if (UserService.Instance.IsUserLoggedIn)
            {
                int userId = UserService.Instance.GetLoggedInUser().Id;
                this.isCardsetInSavedCollection = await this.cardsetService.IsCardsetInUserSavedLibraryAsync(userId, this.cardsetId);
                this.AddCardsetToCollectionButton.Content = this.isCardsetInSavedCollection ? "Видалити з колекції" : "Додати до колекції";
            }

            Log.Information("Cards initialized.");
        }

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Card card)
            {
                this.selectedCard = card;
                this.CardInfo.Text = card.WordEn;
                this.CardComment.Text = string.Empty;

                Log.Information("Card selected.");
            }
        }

        private void CardInfo_Click(object sender, MouseButtonEventArgs e)
        {
            if (this.selectedCard == null)
            {
                Log.Information("Card is not selected.");
            }

            var shrinkAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3),
                AutoReverse = false
            };

            shrinkAnimation.Completed += (s, a) =>
            {
                if (FrontSide.Visibility == Visibility.Visible)
                {
                    FrontSide.Visibility = Visibility.Collapsed;
                    BackSide.Visibility = Visibility.Visible;

                    CardComment.Text = this.selectedCard.WordUa;
                }
                else
                {
                    FrontSide.Visibility = Visibility.Visible;
                    BackSide.Visibility = Visibility.Collapsed;

                    CardInfo.Text = this.selectedCard.WordEn;
                }

                var expandAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.3),
                    AutoReverse = false
                };

                CardFlipTransform.BeginAnimation(ScaleTransform.ScaleXProperty, expandAnimation);
            };

            CardFlipTransform.BeginAnimation(ScaleTransform.ScaleXProperty, shrinkAnimation);
            Log.Information("Card rotated.");
        }

        private async void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var user = UserService.Instance.GetLoggedInUser();
            var cardset = await this.cardsetService.GetCardsetAsync(this.cardsetId);
            if (user != null)
            {
                if (user.Id != cardset.UserRef)
                {
                    try
                    {
                        await this.cardsetService.GetUserscardsetAsync(user.Id, this.cardsetId);
                        this.NavigationService.Navigate(new TestPage(this.cardsetId));
                        Log.Information("Redirecting to TestPage..");
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        Log.Error("Error occurred in TestButton_Click method.", ex);
                        return;
                    }
                }
                else
                {
                    this.NavigationService.Navigate(new TestPage(this.cardsetId));
                    Log.Debug("Redirecting to TestPage.");
                }
            }
            else
            {
                this.NavigationService.Navigate(new LoginPage());
                Log.Debug("Redirecting to LoginPage.");
            }
        }

        private async void EditCardsetButton_Click(object sender, RoutedEventArgs e)
        {
            var user = UserService.Instance.GetLoggedInUser();
            var cardset = await this.cardsetService.GetCardsetAsync(this.cardsetId);
            if (user != null)
            {
                if (user.Id != cardset.UserRef)
                {
                    MessageBox.Show("Ви не є власником цієї колекції.");
                    Log.Warning($"User {user.Id} is not the owner of collection {cardset.Id}.");
                    return;
                }

                this.NavigationService.Navigate(new EditOrCreateCardsetPage(this.cardsetId));
                Log.Debug("Redirecting to EditOrCreateCardsetPage.");
            }
            else
            {
                this.NavigationService.Navigate(new LoginPage());
                Log.Debug("Redirecting to LoginPage.");
            }
        }

        private async void AddCardsetToCollectionButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserService.Instance.IsUserLoggedIn)
            {
                int userId = UserService.Instance.GetLoggedInUser().Id;
                try
                {
                    if (this.isCardsetInSavedCollection)
                    {
                        await this.cardsetService.DeleteUserscardsetAsync(userId, this.cardsetId);
                        Log.Information("Usercardset deleted.");
                        this.isCardsetInSavedCollection = false;
                        this.AddCardsetToCollectionButton.Content = "Додати до колекції";
                    }
                    else
                    {
                        await this.cardsetService.AddCardsetToSavedLibraryAsync(userId, this.cardsetId);
                        Log.Information("Cardset saved to library.");
                        this.isCardsetInSavedCollection = true;
                        this.AddCardsetToCollectionButton.Content = "Видалити з колекції";
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Log.Error("Error occurred in AddCardsetToCollectionButton_Click method.", ex);
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                this.NavigationService.Navigate(new LoginPage());
                Log.Debug("Redirecting to LoginPage.");
            }
        }


        private async void DeleteCardsetButton_Click(object sender, RoutedEventArgs e)
        {
            var user = UserService.Instance.GetLoggedInUser();
            var cardset = await this.cardsetService.GetCardsetAsync(this.cardsetId);
            if (user != null && user.Id == cardset.UserRef)
            {
                try
                {
                    var result = MessageBox.Show(
                        "Ви впевнені, що хочете видалити цю колекцію?",
                        "Підтвердження",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        int userId = user.Id;
                        await this.cardsetService.DeleteCardsetAsync(userId, this.cardsetId);
                        Log.Information("Cardset deleted.");
                        this.NavigationService.Navigate(new OwnedCardsetLibraryPage());
                        Log.Debug("Redirecting to OwnedCardsetLibraryPage.");
                    }
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Log.Error("Error occurred in DeleteCardsetButton_Click method.", ex);
                }
            }
            else
            {
                MessageBox.Show("Ви не є власником цієї колекції.");
                Log.Information($"User {user.Id} is not the owner of collection {cardset.Id}.");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
            Log.Debug("Redirecting to GoBack.");
        }
    }
}
