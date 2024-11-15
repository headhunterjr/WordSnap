using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using WordSnapWPFApp.BLL.Services;
using WordSnapWPFApp.BLL.ViewModels;

namespace WordSnapWPFApp.Presentation.Pages
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : Page
    {
        private readonly CardsetService _cardsetService = new CardsetService();
        private TestViewModel _viewModel;
        private string _selectedWordEn;
        private int _cardsetId;

        public TestPage(int cardsetId)
        {
            InitializeComponent();
            _cardsetId = cardsetId;
            InitializePageAsync(cardsetId);
        }

        private async void InitializePageAsync(int cardsetId)
        {
            var cards = await _cardsetService.GetCardsOfCardsetForTestAsync(cardsetId);
            _viewModel = new TestViewModel
            {
                Cards = cards.ToList(),
            };

            DataContext = new
            {
                WordEnButtons = _viewModel.Cards.Select(c => new { c.WordEn, Text = c.WordEn }).ToList(),
                WordUaButtons = _viewModel.Cards.OrderBy(_ => Guid.NewGuid())
                                                 .Select(c => new { c.WordUa, Text = c.WordUa }).ToList()
            };
        }

        private void WordEnButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string wordEn)
            {
                _selectedWordEn = wordEn;
                button.BorderBrush = Brushes.Blue;
            }
        }

        private async void WordUaButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedWordEn)) return;

            if (sender is Button button && button.Tag is string wordUa)
            {
                try
                {
                    // Check if the selected English word matches the Ukrainian word
                    var isCorrect = _viewModel.MakeGuess(_selectedWordEn, wordUa);

                    if (isCorrect)
                    {
                        // Correct match
                        button.IsEnabled = false;
                        var wordEnButton = FindButtonByText(_selectedWordEn);
                        if (wordEnButton != null) wordEnButton.IsEnabled = false;

                        button.BorderBrush = Brushes.Green;
                        wordEnButton.BorderBrush = Brushes.Green;
                    }
                    else
                    {
                        // Incorrect match
                        await AnimateBorder(button, Brushes.Red);
                        var wordEnButton = FindButtonByText(_selectedWordEn);
                        if (wordEnButton != null) await AnimateBorder(wordEnButton, Brushes.Red);
                    }

                    // Check if the test is complete
                    if (_viewModel.IsTestComplete)
                    {
                        await SaveResultsAsync();
                    }
                }
                finally
                {
                    _selectedWordEn = null;
                }
            }
        }

        private async Task SaveResultsAsync()
        {
            var userId = UserService.Instance.GetLoggedInUser().Id;
            var cardsetId = _cardsetId;
            await _cardsetService.SaveTestProgressAsync(userId, cardsetId, _viewModel.Accuracy);
        }

        private async Task AnimateBorder(Button button, Brush color)
        {
            button.BorderBrush = color;
            await Task.Delay(500);
            button.BorderBrush = Brushes.Transparent;
        }

        private Button FindButtonByText(string text)
        {
            return FindVisualChildren<Button>(this).FirstOrDefault(b => b.Content.ToString() == text);
        }

        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T t)
                    yield return t;

                foreach (var childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
