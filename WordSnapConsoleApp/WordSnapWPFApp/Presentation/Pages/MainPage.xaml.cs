﻿using System;
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
using WordSnapWPFApp.DAL.Models;

namespace WordSnapWPFApp.Presentation.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly WordSnapRepository _repository = new WordSnapRepository();
        private TextBox[] cardsetTextBoxes;
        public MainPage()
        {
            InitializeComponent();
            InitializeRabdomCardsets();
        }

        private async void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var cardsets = await _repository.GetCardsetsFromSearch(SearchBox.Text);
            var cardsetList = cardsets.ToList();
            TextBox[] cardsetTextBoxes = {Card1TextBox, Card2TextBox, Card3TextBox};
            for (int i = 0; i < cardsetTextBoxes.Length; ++i)
            {
                if (cardsetList[i] != null)
                {
                    cardsetTextBoxes[i].Text = cardsetList[i].Name;
                }
            }
        }
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = SearchBox.Text;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var cardsets = await _repository.GetCardsetsFromSearch(searchQuery);

                int i = 0;
                foreach (var cardset in cardsets.Take(3))
                {
                    cardsetTextBoxes[i].Text = cardset.Name;
                    i++;
                }

                for (; i < cardsetTextBoxes.Length; i++)
                {
                    cardsetTextBoxes[i].Text = string.Empty;
                }
            }
        }
        private async void InitializeRabdomCardsets()
        {
            cardsetTextBoxes = [Card1TextBox, Card2TextBox, Card3TextBox];
            var cardsets = await _repository.GetRandomCardsets();

            int i = 0;
            foreach (var cardset in cardsets.Take(3))
            {
                cardsetTextBoxes[i].Text = cardset.Name;
                i++;
            }
        }
    }
}