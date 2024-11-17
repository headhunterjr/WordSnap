using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WordSnapWPFApp.BLL.Services;
using WordSnapWPFApp.DAL.Models;

namespace WordSnapWPFApp.Presentation.Pages
{
    public partial class EditOrCreateCardsetPage : Page
    {
        private readonly CardsetService _cardsetService = new CardsetService();
        private readonly ValidationService _validationService = new ValidationService();
        private int? _cardsetId;
        private Cardset _currentCardset;
        private Card? _selectedCard;
        private ObservableCollection<Card> _observedCards;
        private bool _isInitializing = true;

        public EditOrCreateCardsetPage(int? cardsetId = null)
        {
            InitializeComponent();
            _cardsetId = cardsetId;
            _observedCards = new ObservableCollection<Card>();
            CardsListBox.ItemsSource = _observedCards;
            InitializePage();
        }

        private async void InitializePage()
        {
            if (_cardsetId.HasValue)
            {
                _currentCardset = await _cardsetService.GetCardsetAsync(_cardsetId.Value);
                if (_currentCardset != null)
                {
                    CardsetName.Text = _currentCardset.Name;
                    var cards = await _cardsetService.GetCardsOfCardsetAsync(_cardsetId.Value);
                    _observedCards.Clear();
                    foreach (var card in cards)
                    {
                        _observedCards.Add(card);
                    }
                    CardsetToggle.IsChecked = !_currentCardset.IsPublic;
                }
            }
            else
            {
                _currentCardset = new Cardset
                {
                    Name = "Нова колекція",
                    IsPublic = false,
                    UserRef = UserService.Instance.GetLoggedInUser().Id,
                    Cards = new List<Card>()
                };
                await _cardsetService.CreateCardsetAsync(_currentCardset);
                _cardsetId = _currentCardset.Id; 

                CardsetName.Text = _currentCardset.Name;
                CardsetToggle.IsChecked = true;
            }

            CardsetName.LostFocus += CardsetName_LostFocus;
            _isInitializing = false;
        }

        private async void CardsetName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_isInitializing || _currentCardset == null ||
                _currentCardset.Name == CardsetName.Text) return;
            var validationResult = _validationService.ValidateEnglishText(CardsetName.Text, true);
            if (!validationResult.IsValid)
            {
                MessageBox.Show(validationResult.ErrorMessage, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                CardsetName.Text = _currentCardset.Name;
                return;
            }
            _currentCardset.Name = CardsetName.Text;
            await _cardsetService.UpdateCardsetAsync(_currentCardset);
        }

        private async void CardsetToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (_currentCardset != null && !_isInitializing)
            {
                _currentCardset.IsPublic = !CardsetToggle.IsChecked;
                await _cardsetService.UpdateCardsetAsync(_currentCardset);
            }
        }

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Card card)
            {
                _selectedCard = card;
                UpdateEditForm();
            }
        }

        private void UpdateEditForm()
        {
            if (_selectedCard != null)
            {
                WordEnTextBox.Text = _selectedCard.WordEn;
                WordUaTextBox.Text = _selectedCard.WordUa;
                CommentTextBox.Text = _selectedCard.Comment;
            }
        }

        private void AddNewCard_Click(object sender, RoutedEventArgs e)
        {
            _selectedCard = null;
            WordEnTextBox.Text = string.Empty;
            WordUaTextBox.Text = string.Empty;
            CommentTextBox.Text = string.Empty;
        }

        private async void AddOrUpdateCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(WordEnTextBox.Text) || string.IsNullOrWhiteSpace(WordUaTextBox.Text))
            {
                MessageBox.Show("Спершу введіть слово та його переклад.");
                return;
            }
            var englishValidation = _validationService.ValidateEnglishText(WordEnTextBox.Text);
            var ukrainianValidation = _validationService.ValidateUkrainianText(WordUaTextBox.Text);
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

            if (_selectedCard == null)
            {
                var newCard = new Card
                {
                    WordEn = WordEnTextBox.Text,
                    WordUa = WordUaTextBox.Text,
                    Comment = CommentTextBox.Text
                };

                await _cardsetService.AddCardToCardsetAsync(newCard, _cardsetId.Value);

                _observedCards.Add(newCard);
            }
            else
            {
                _selectedCard.WordEn = WordEnTextBox.Text;
                _selectedCard.WordUa = WordUaTextBox.Text;
                _selectedCard.Comment = CommentTextBox.Text;

                await _cardsetService.UpdateCardAsync(_selectedCard);

                var cardIndex = _observedCards.IndexOf(_selectedCard);
                if (cardIndex != -1)
                {
                    _observedCards[cardIndex] = _selectedCard;
                }
            }

            _selectedCard = null;
            WordEnTextBox.Text = string.Empty;
            WordUaTextBox.Text = string.Empty;
            CommentTextBox.Text = string.Empty;
        }
        private async void DeleteCardButton_Click(object sender, RoutedEventArgs e)
        {
            var user = UserService.Instance.GetLoggedInUser();
            if (user != null)
            {
                try
                {
                    var result = MessageBox.Show("Ви впевнені, що хочете видалити цю картку?",
                        "Підтвердження",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        int cardId = ((Button)sender).Tag as int? ?? throw new InvalidOperationException("ID картки не знайдено.");
                        await _cardsetService.DeleteCardAsync(user.Id, cardId);
                        NavigationService.Refresh();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                NavigationService.Navigate(new LoginPage());
            }
        }
    }
}