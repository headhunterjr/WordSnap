// <copyright file="EditOrCreateCardsetPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.Presentation.Pages
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using WordSnapWPFApp.BLL.Services;
    using WordSnapWPFApp.DAL.Models;

    /// <summary>
    /// edit or create a cardset page.
    /// </summary>
    public partial class EditOrCreateCardsetPage : Page
    {
        private readonly CardsetService cardsetService = new CardsetService();
        private readonly ValidationService validationService = new ValidationService();
        private int? cardsetId;
        private Cardset currentCardset;
        private Card? selectedCard;
        private ObservableCollection<Card> observedCards;
        private bool isInitializing = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditOrCreateCardsetPage"/> class.
        /// </summary>
        /// <param name="cardsetId">cardset's Id.</param>
        public EditOrCreateCardsetPage(int? cardsetId = null)
        {
            this.InitializeComponent();
            this.cardsetId = cardsetId;
            this.observedCards = new ObservableCollection<Card>();
            this.CardsListBox.ItemsSource = this.observedCards;
            this.InitializePage();
        }

        private async void InitializePage()
        {
            if (this.cardsetId.HasValue)
            {
                this.currentCardset = await this.cardsetService.GetCardsetAsync(this.cardsetId.Value);
                if (this.currentCardset != null)
                {
                    this.CardsetName.Text = this.currentCardset.Name;
                    var cards = await this.cardsetService.GetCardsOfCardsetAsync(this.cardsetId.Value);
                    this.observedCards.Clear();
                    foreach (var card in cards)
                    {
                        this.observedCards.Add(card);
                    }

                    this.CardsetToggle.IsChecked = !this.currentCardset.IsPublic;
                }
            }
            else
            {
                this.currentCardset = new Cardset
                {
                    Name = "Нова колекція",
                    IsPublic = false,
                    UserRef = UserService.Instance.GetLoggedInUser().Id,
                    Cards = new List<Card>(),
                };
                await this.cardsetService.CreateCardsetAsync(this.currentCardset);
                this.cardsetId = this.currentCardset.Id;

                this.CardsetName.Text = this.currentCardset.Name;
                this.CardsetToggle.IsChecked = true;
            }

            this.CardsetName.LostFocus += this.CardsetName_LostFocus;
            this.isInitializing = false;
        }

        private async void CardsetName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.isInitializing || this.currentCardset == null ||
                this.currentCardset.Name == this.CardsetName.Text)
            {
                return;
            }

            var validationResult = this.validationService.ValidateEnglishText(this.CardsetName.Text, true);
            if (!validationResult.IsValid)
            {
                MessageBox.Show(validationResult.ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.CardsetName.Text = this.currentCardset.Name;
                return;
            }

            this.currentCardset.Name = this.CardsetName.Text;
            await this.cardsetService.UpdateCardsetAsync(this.currentCardset);
        }

        private async void CardsetToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (this.currentCardset != null && !this.isInitializing)
            {
                this.currentCardset.IsPublic = !this.CardsetToggle.IsChecked;
                await this.cardsetService.UpdateCardsetAsync(this.currentCardset);
            }
        }

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Card card)
            {
                this.selectedCard = card;
                this.UpdateEditForm();
            }
        }

        private void UpdateEditForm()
        {
            if (this.selectedCard != null)
            {
                this.WordEnTextBox.Text = this.selectedCard.WordEn;
                this.WordUaTextBox.Text = this.selectedCard.WordUa;
                this.CommentTextBox.Text = this.selectedCard.Comment;
            }
        }

        private void AddNewCard_Click(object sender, RoutedEventArgs e)
        {
            this.selectedCard = null;
            this.WordEnTextBox.Text = string.Empty;
            this.WordUaTextBox.Text = string.Empty;
            this.CommentTextBox.Text = string.Empty;
        }

        private async void AddOrUpdateCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.WordEnTextBox.Text) || string.IsNullOrWhiteSpace(this.WordUaTextBox.Text))
            {
                MessageBox.Show("Спершу введіть слово та його переклад.");
                return;
            }

            var englishValidation = this.validationService.ValidateEnglishText(this.WordEnTextBox.Text);
            var ukrainianValidation = this.validationService.ValidateUkrainianText(this.WordUaTextBox.Text);
            if (!englishValidation.IsValid)
            {
                MessageBox.Show(englishValidation.ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ukrainianValidation.IsValid)
            {
                MessageBox.Show(ukrainianValidation.ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (this.selectedCard == null)
            {
                var newCard = new Card
                {
                    WordEn = this.WordEnTextBox.Text,
                    WordUa = this.WordUaTextBox.Text,
                    Comment = this.CommentTextBox.Text,
                };

                await this.cardsetService.AddCardToCardsetAsync(newCard, this.cardsetId.Value);

                this.observedCards.Add(newCard);
            }
            else
            {
                this.selectedCard.WordEn = this.WordEnTextBox.Text;
                this.selectedCard.WordUa = this.WordUaTextBox.Text;
                this.selectedCard.Comment = this.CommentTextBox.Text;

                await this.cardsetService.UpdateCardAsync(this.selectedCard);

                var cardIndex = this.observedCards.IndexOf(this.selectedCard);
                if (cardIndex != -1)
                {
                    this.observedCards[cardIndex] = this.selectedCard;
                }
            }

            this.selectedCard = null;
            this.WordEnTextBox.Text = string.Empty;
            this.WordUaTextBox.Text = string.Empty;
            this.CommentTextBox.Text = string.Empty;
        }

        private async void DeleteCardButton_Click(object sender, RoutedEventArgs e)
        {
            var user = UserService.Instance.GetLoggedInUser();
            if (user == null)
            {
                this.NavigationService.Navigate(new LoginPage());
                return;
            }

            if (this.selectedCard == null)
            {
                MessageBox.Show(
                    "Спочатку виберіть картку для видалення.",
                    "Попередження",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            await this.TryDeleteCardAsync(this.selectedCard, user.Id);
        }

        private async void DeleteCardMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var user = UserService.Instance.GetLoggedInUser();
            if (user == null)
            {
                this.NavigationService.Navigate(new LoginPage());
                return;
            }

            var menuItem = sender as MenuItem;
            var contextMenu = menuItem?.Parent as ContextMenu;
            var button = contextMenu?.PlacementTarget as Button;
            var cardToDelete = button?.DataContext as Card;

            if (cardToDelete == null)
            {
                MessageBox.Show(
                    "Не вдалося знайти картку для видалення.",
                    "Помилка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            await this.TryDeleteCardAsync(cardToDelete, user.Id);
        }

        private async Task TryDeleteCardAsync(Card cardToDelete, int userId)
        {
            var result = MessageBox.Show(
                "Ви впевнені, що хочете видалити цю картку?",
                "Підтвердження",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await this.cardsetService.DeleteCardAsync(userId, cardToDelete.Id);
                    this.observedCards.Remove(cardToDelete);

                    if (this.selectedCard?.Id == cardToDelete.Id)
                    {
                        this.selectedCard = null;
                        this.WordEnTextBox.Text = string.Empty;
                        this.WordUaTextBox.Text = string.Empty;
                        this.CommentTextBox.Text = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}