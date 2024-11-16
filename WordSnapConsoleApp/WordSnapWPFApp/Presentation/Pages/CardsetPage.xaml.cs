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
            NavigationService.Navigate(new LoginPage());
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
        }
    }
}
