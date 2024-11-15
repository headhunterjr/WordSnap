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
    /// Interaction logic for OwnedCardsetLibraryPage.xaml
    /// </summary>
    public partial class OwnedCardsetLibraryPage : Page
    {
        private readonly CardsetService _cardsetService = new CardsetService();
        public OwnedCardsetLibraryPage()
        {
            InitializeComponent();
            InitializeCardsets();
        }

        private async void InitializeCardsets()
        {
            int userId = UserService.Instance.GetLoggedInUser().Id;
            var cardsets = await _cardsetService.GetUsersOwnCardsetsLibraryAsync(userId);
            CardsetsListBox.ItemsSource = cardsets;
        }

        private void CardsetButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Cardset cardset)
            {
                NavigationService.Navigate(new CardsetPage(cardset.Id, cardset.Name));
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
