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
    /// Interaction logic for CardsetPage.xaml
    /// </summary>
    public partial class CardsetPage : Page
    {
        private readonly CardsetService _cardsetService = new CardsetService();
        private int _cardsetId;
        private string _cardsetName;
        private Card _selectedCard;
        public CardsetPage(int cardsetId, string cardsetName)
        {
            InitializeComponent();
            _cardsetId = cardsetId;
            _cardsetName = cardsetName;
            InitializeCards();
            UpdateUIForLoginState();
        }

        private async void InitializeCards()
        {
            var cards = await _cardsetService.GetCardsOfCardsetAsync(_cardsetId);
            CardsListBox.ItemsSource = cards;
            CardsetName.Text = _cardsetName;
        }

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Card card)
            {
                _selectedCard = card;
                CardInfo.Text = card.WordEn;
            }
        }

        private void CardInfo_Click(object sender, MouseButtonEventArgs e)
        {
            if (_selectedCard != null)
            {
                if (CardInfo.Text == _selectedCard.WordEn)
                {
                    CardInfo.Text = _selectedCard.WordUa;
                }
                else
                {
                    CardInfo.Text = _selectedCard.WordEn;
                }
            }
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserService.Instance.IsUserLoggedIn)
            {
                NavigationService.Navigate(new TestPage(_cardsetId));
            }
            else
            {
                NavigationService.Navigate(new LoginPage());
            }
        }
        private async void EditCardsetButton_Click(object sender, RoutedEventArgs e)
        {
            var user = UserService.Instance.GetLoggedInUser();
            var cardset = await _cardsetService.GetCardsetAsync(_cardsetId);
            if (user != null)
            {
                if (user.Id != cardset.UserRef)
                {
                    MessageBox.Show("Ви не є власником цієї колекції.");
                    return;
                }
                NavigationService.Navigate(new EditOrCreateCardsetPage(_cardsetId));
            }
            else
            {
                NavigationService.Navigate(new LoginPage());
            }
        }

        private async void AddCardsetToCollectionButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserService.Instance.IsUserLoggedIn)
            {
                try
                {
                    int userId = UserService.Instance.GetLoggedInUser().Id;
                    int cardsetId = _cardsetId;
                    await _cardsetService.AddCardsetToSavedLibraryAsync(userId, cardsetId);
                }
                catch(InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                NavigationService.Navigate(new LoginPage());
            }
        }

        private async void DeleteCardsetButton_Click(object sender, RoutedEventArgs e)
        {
            var user = UserService.Instance.GetLoggedInUser();
            var cardset = await _cardsetService.GetCardsetAsync(_cardsetId);
            if (user != null && user.Id == cardset.UserRef)
            {
                try
                {
                    var result = MessageBox.Show("Ви впевнені, що хочете видалити цю колекцію?",
                        "Підтвердження",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        int userId = user.Id;
                        await _cardsetService.DeleteCardsetAsync(userId, _cardsetId);
                        NavigationService.Navigate(new OwnedCardsetLibraryPage());
                    }
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Ви не є власником цієї колекції.");
            }
        }

        public void UpdateUIForLoginState()
        {
            if (UserService.Instance.IsUserLoggedIn)
            {
                DeleteCardsetButton.Visibility = Visibility.Visible;
                ActionButton.Visibility = Visibility.Visible;
                EditCardsetButton.Visibility = Visibility.Visible;
                AddCardsetToCollectionButton.Visibility = Visibility.Visible;
            }
            else
            {
                DeleteCardsetButton.Visibility = Visibility.Hidden;
                ActionButton.Visibility = Visibility.Hidden;
                EditCardsetButton.Visibility = Visibility.Hidden;
                AddCardsetToCollectionButton.Visibility = Visibility.Hidden;
            }
        }
    }
}
