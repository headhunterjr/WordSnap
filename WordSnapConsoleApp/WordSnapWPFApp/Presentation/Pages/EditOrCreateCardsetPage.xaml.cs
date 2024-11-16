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
        public Cardset Cardset { get; private set; }
        public Card SelectedCard { get; private set; } = new Card();

        public string PrivacyButtonText => (bool) Cardset.IsPublic ? "Public" : "Private";
        public Brush PrivacyButtonColor => (bool) Cardset.IsPublic ? Brushes.Green : Brushes.Red;

        public EditOrCreateCardsetPage(int? cardsetId = null)
        {
            InitializeComponent();
            LoadCardset(cardsetId);
        }

        private async void LoadCardset(int? cardsetId)
        {
            if (cardsetId.HasValue)
            {
                Cardset = await _cardsetService.GetCardsetAsync(cardsetId.Value);
            }
            else
            {
                Cardset = new Cardset { Name = "", IsPublic = false, Cards = new ObservableCollection<Card>() };
            }

            DataContext = this;
        }

        private async void ChangeNameButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Cardset.Name))
            {
                await _cardsetService.UpdateCardsetNameAsync(Cardset.Id, Cardset.Name);
                MessageBox.Show("Cardset name updated!");
            }
        }

        private async void AddOrUpdateCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SelectedCard.WordEn) && !string.IsNullOrWhiteSpace(SelectedCard.WordUa))
            {
                if (!Cardset.Cards.Contains(SelectedCard))
                {
                    Cardset.Cards.Add(SelectedCard);
                }

                //await _cardsetService.AddOrUpdateCardAsync(Cardset.Id, SelectedCard);
                MessageBox.Show("Card added/updated!");
            }
        }

        private async void DeleteCardsetButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this cardset?",
                "Confirm Deletion", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                //await _cardsetService.DeleteCardsetAsync(Cardset.Id);
                MessageBox.Show("Cardset deleted!");
                NavigationService.GoBack();
            }
        }
    }
}
