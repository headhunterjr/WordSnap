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
        private int? _cardsetId;
        private Cardset _currentCardset;
        private Card? _selectedCard;
        private ObservableCollection<Card> _observedCards;

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
                    Cards = new List<Card>()
                };
                CardsetName.Text = _currentCardset.Name;
                CardsetToggle.IsChecked = true;
            }
        }

        private async void CardsetToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (_currentCardset != null)
            {
                _currentCardset.IsPublic = !CardsetToggle.IsChecked;
                if (_cardsetId.HasValue)
                {
                    await _cardsetService.UpdateCardsetAsync(_currentCardset);
                }
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

            if (_selectedCard == null)
            {
                var newCard = new Card
                {
                    WordEn = WordEnTextBox.Text,
                    WordUa = WordUaTextBox.Text,
                    Comment = CommentTextBox.Text,
                    Id = _cardsetId ?? 0
                };

                if (_cardsetId.HasValue)
                {
                    await _cardsetService.AddCardToCardsetAsync(newCard, _cardsetId.Value);
                    _observedCards.Add(newCard);
                }
                else
                {
                    _observedCards.Add(newCard);
                }
            }
            else
            {
                _selectedCard.WordEn = WordEnTextBox.Text;
                _selectedCard.WordUa = WordUaTextBox.Text;
                _selectedCard.Comment = CommentTextBox.Text;

                if (_cardsetId.HasValue)
                {
                   await _cardsetService.UpdateCardAsync(_selectedCard);
                }

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

        private async void SaveCardset_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CardsetName.Text))
            {
                MessageBox.Show("Введіть назву колекції.");
                return;
            }

            _currentCardset.Name = CardsetName.Text;
            _currentCardset.IsPublic = CardsetToggle.IsChecked ?? false;

            if (_cardsetId.HasValue)
            {
                await _cardsetService.UpdateCardsetAsync(_currentCardset);
            }
            else
            {
                _currentCardset.UserRef = UserService.Instance.GetLoggedInUser().Id;
                _currentCardset.Cards = new List<Card>(_observedCards);

                await _cardsetService.CreateCardsetAsync(_currentCardset);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cardsetId.HasValue)
            {
                NavigationService.Navigate(new CardsetPage(_cardsetId.Value, _currentCardset.Name));
            }
            else
            {
                NavigationService.GoBack();
            }
        }
    }
}
