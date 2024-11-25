// <copyright file="MainPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.Presentation.Pages
{
    using Serilog;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using WordSnapWPFApp.BLL.Services;
    using WordSnapWPFApp.DAL.Models;

    /// <summary>
    /// Interaction logic for MainPage.xaml.
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly CardsetService cardsetService = new CardsetService();
        private TextBox[] cardsetTextBoxes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.InitializeRabdomCardsets();
            Log.Information("MainPage loaded.");
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = this.SearchBox.Text;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var cardsets = await this.cardsetService.GetCardsetsFromSearchAsync(searchQuery);

                var buttons = new[] { this.Card1TextBox.Parent as Button, this.Card2TextBox.Parent as Button, this.Card3TextBox.Parent as Button };

                int i = 0;
                foreach (var cardset in cardsets)
                {
                    if (i < buttons.Length && buttons[i] != null)
                    {
                        buttons[i].Tag = cardset;
                        this.cardsetTextBoxes[i].Text = cardset.Name;
                        i++;
                    }
                }

                for (; i < buttons.Length; i++)
                {
                    if (buttons[i] != null)
                    {
                        buttons[i].Tag = null;
                        this.cardsetTextBoxes[i].Text = string.Empty;
                    }
                }

                Log.Information("Return searching result");
            }

            Log.Information("Search query is empty.");
        }

        private async void InitializeRabdomCardsets()
        {
            this.cardsetTextBoxes = new TextBox[3];
            this.cardsetTextBoxes[0] = this.Card1TextBox;
            this.cardsetTextBoxes[1] = this.Card2TextBox;
            this.cardsetTextBoxes[2] = this.Card3TextBox;

            var buttons = new[] { this.Card1TextBox.Parent as Button, this.Card2TextBox.Parent as Button, this.Card3TextBox.Parent as Button };
            var cardsets = await this.cardsetService.GetRandomCardsetsAsync();

            int i = 0;
            foreach (var cardset in cardsets)
            {
                if (i < buttons.Length)
                {
                    buttons[i].Tag = cardset;
                    this.cardsetTextBoxes[i].Text = cardset.Name;
                    i++;
                }
            }

            Log.Information("Random cardsets initialized.");
        }

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Cardset cardset)
            {
                this.NavigationService.Navigate(new CardsetPage(cardset.Id, cardset.Name));
            }
        }
    }
}
