// <copyright file="TestPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.Presentation.Pages
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Navigation;
    using WordSnapWPFApp.BLL.Services;
    using WordSnapWPFApp.BLL.ViewModels;

    /// <summary>
    /// Interaction logic for TestPage.xaml.
    /// </summary>
    public partial class TestPage : Page
    {
        private readonly CardsetService cardsetService = new CardsetService();
        private TestViewModel viewModel;
        private string selectedWordEn;
        private int cardsetId;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestPage"/> class.
        /// </summary>
        /// <param name="cardsetId">cardset's Id.</param>
        public TestPage(int cardsetId)
        {
            this.InitializeComponent();
            this.cardsetId = cardsetId;
            this.InitializePageAsync(cardsetId);
        }

        private async void InitializePageAsync(int cardsetId)
        {
            var cards = await this.cardsetService.GetCardsOfCardsetForTestAsync(cardsetId);
            this.viewModel = new TestViewModel
            {
                Cards = cards.ToList(),
            };

            this.DataContext = new
            {
                WordEnButtons = this.viewModel.Cards.Select(c => new { c.WordEn, Text = c.WordEn }).ToList(),
                WordUaButtons = this.viewModel.Cards.OrderBy(_ => Guid.NewGuid())
                                                 .Select(c => new { c.WordUa, Text = c.WordUa }).ToList(),
            };
        }

        private void WordEnButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string wordEn)
            {
                foreach (var btn in this.FindVisualChildren<Button>(this))
                {
                    if (btn.BorderBrush == Brushes.Blue)
                    {
                        btn.BorderBrush = Brushes.Transparent;
                    }
                }

                this.selectedWordEn = wordEn;
                button.BorderBrush = Brushes.Blue;
            }
        }

        private async void WordUaButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.selectedWordEn))
            {
                return;
            }

            if (sender is Button button && button.Tag is string wordUa)
            {
                try
                {
                    var isCorrect = this.viewModel.MakeGuess(this.selectedWordEn, wordUa);

                    if (isCorrect)
                    {
                        button.IsEnabled = false;
                        var wordEnButton = this.FindButtonByText(this.selectedWordEn);
                        if (wordEnButton != null)
                        {
                            wordEnButton.IsEnabled = false;
                        }

                        button.BorderBrush = Brushes.Green;
                        if (wordEnButton != null)
                        {
                            wordEnButton.BorderBrush = Brushes.Green;
                        }
                    }
                    else
                    {
                        await this.AnimateBorder(button, Brushes.Red);
                        var wordEnButton = this.FindButtonByText(this.selectedWordEn);
                        if (wordEnButton != null)
                        {
                            await this.AnimateBorder(wordEnButton, Brushes.Red);
                        }
                    }

                    if (this.viewModel.IsTestComplete)
                    {
                        await this.SaveResultsAsync();

                        MessageBox.Show($"Вітання! Ви пройшли тест з результатом {Math.Round(this.viewModel.Accuracy * 100, 2)}%!");

                        this.ResetTestUI();
                    }
                }
                finally
                {
                    this.selectedWordEn = null;
                }
            }
        }

        private void ResetTestUI()
        {
            this.selectedWordEn = null;
        }

        private async Task SaveResultsAsync()
        {
            var userId = UserService.Instance.GetLoggedInUser().Id;
            var cardsetId = this.cardsetId;
            await this.cardsetService.SaveTestProgressAsync(userId, cardsetId, this.viewModel.Accuracy);
        }

        private async Task AnimateBorder(Button button, Brush color)
        {
            button.BorderBrush = color;
            await Task.Delay(500);
            button.BorderBrush = Brushes.Transparent;
        }

        private Button FindButtonByText(string text)
        {
            return this.FindVisualChildren<Button>(this).FirstOrDefault(b =>
            {
                if (b.Content is string buttonText)
                {
                    return buttonText == text;
                }

                return false;
            });
        }

        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
            where T : DependencyObject
        {
            if (depObj == null)
            {
                yield break;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T t)
                {
                    yield return t;
                }

                foreach (var childOfChild in this.FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
