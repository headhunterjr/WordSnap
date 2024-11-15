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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly CardsetService _cardsetService = new CardsetService();
        private TextBox[] cardsetTextBoxes;
        public MainPage()
        {
            InitializeComponent();
            InitializeRabdomCardsets();
        }
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = SearchBox.Text;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var cardsets = await _cardsetService.GetCardsetsFromSearchAsync(searchQuery);

                var buttons = new[] { Card1TextBox.Parent as Button, Card2TextBox.Parent as Button, Card3TextBox.Parent as Button };

                int i = 0;
                foreach (var cardset in cardsets)
                {
                    if (i < buttons.Length && buttons[i] != null)
                    {
                        buttons[i].Tag = cardset;
                        cardsetTextBoxes[i].Text = cardset.Name;
                        i++;
                    }
                }

                for (; i < buttons.Length; i++)
                {
                    if (buttons[i] != null)
                    {
                        buttons[i].Tag = null;
                        cardsetTextBoxes[i].Text = string.Empty;
                    }
                }
            }
        }

        private async void InitializeRabdomCardsets()
        {
            cardsetTextBoxes = [Card1TextBox, Card2TextBox, Card3TextBox];
            var buttons = new[] { Card1TextBox.Parent as Button, Card2TextBox.Parent as Button, Card3TextBox.Parent as Button };
            var cardsets = await _cardsetService.GetRandomCardsetsAsync();

            int i = 0;
            foreach (var cardset in cardsets)
            {
                if (i < buttons.Length)
                {
                    buttons[i].Tag = cardset;
                    cardsetTextBoxes[i].Text = cardset.Name;
                    i++;
                }
            }
        }

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Cardset cardset)
            {
                NavigationService.Navigate(new CardsetPage(cardset.Id, cardset.Name));
            }
        }
    }
}
