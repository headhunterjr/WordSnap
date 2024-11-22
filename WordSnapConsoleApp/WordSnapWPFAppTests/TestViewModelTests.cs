// <copyright file="TestViewModelTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFAppTests
{
    using WordSnapWPFApp.BLL.ViewModels;
    using WordSnapWPFApp.DAL.Models;

    /// <summary>
    /// Test view model tests.
    /// </summary>
    public class TestViewModelTests
    {
        /// <summary>
        /// Accuracy should be zero without attempts.
        /// </summary>
        [Fact]
        public void Accuracy_ShouldBeZero_WhenNoAttemptsMade()
        {
            // Arrange
            var viewModel = new TestViewModel();

            // Act
            var accuracy = viewModel.Accuracy;

            // Assert
            Assert.Equal(0.0, accuracy);
        }

        /// <summary>
        /// Accuracy should be calculated correctly.
        /// </summary>
        [Fact]
        public void Accuracy_ShouldBeCorrect_WhenAttemptsMade()
        {
            // Arrange
            var viewModel = new TestViewModel
            {
                Cards = new List<Card>
                {
                    new Card { WordEn = "One", WordUa = "Один" },
                    new Card { WordEn = "Two", WordUa = "Два" },
                },
            };

            // Act
            viewModel.MakeGuess("One", "Один");
            viewModel.MakeGuess("Two", "Три");
            var accuracy = viewModel.Accuracy;

            // Assert
            Assert.Equal(0.5, accuracy);
        }

        /// <summary>
        /// Test should be complete when all cards are matched.
        /// </summary>
        [Fact]
        public void IsTestComplete_ShouldReturnTrue_WhenAllCardsMatched()
        {
            // Arrange
            var viewModel = new TestViewModel
            {
                Cards = new List<Card>
                {
                    new Card { WordEn = "One", WordUa = "Один" },
                },
            };

            // Act
            viewModel.MakeGuess("One", "Один");
            var isComplete = viewModel.IsTestComplete;

            // Assert
            Assert.True(isComplete);
        }

        /// <summary>
        /// Test should not be complete if any cards are not matched.
        /// </summary>
        [Fact]
        public void IsTestComplete_ShouldReturnFalse_WhenNotAllCardsMatched()
        {
            // Arrange
            var viewModel = new TestViewModel
            {
                Cards = new List<Card>
                {
                    new Card { WordEn = "One", WordUa = "Один" },
                    new Card { WordEn = "Two", WordUa = "Два" },
                },
            };

            // Act
            viewModel.MakeGuess("One", "Один");
            var isComplete = viewModel.IsTestComplete;

            // Assert
            Assert.False(isComplete);
        }

        /// <summary>
        /// MakeGuess method should return true when a guess is correct.
        /// </summary>
        [Fact]
        public void MakeGuess_ShouldReturnTrue_WhenCorrectGuessMade()
        {
            // Arrange
            var viewModel = new TestViewModel
            {
                Cards = new List<Card>
                {
                    new Card { WordEn = "One", WordUa = "Один" },
                },
            };

            // Act
            var result = viewModel.MakeGuess("One", "Один");

            // Assert
            Assert.True(result);
            Assert.Equal(1, viewModel.CorrectAttempts);
            Assert.Equal(1, viewModel.TotalAttempts);
            Assert.Contains("One", viewModel.Matches.Keys);
        }

        /// <summary>
        /// MakeGuess method should return false when an incorrect guess is made.
        /// </summary>
        [Fact]
        public void MakeGuess_ShouldReturnFalse_WhenIncorrectGuessMade()
        {
            // Arrange
            var viewModel = new TestViewModel
            {
                Cards = new List<Card>
                {
                    new Card { WordEn = "One", WordUa = "Один" },
                },
            };

            // Act
            var result = viewModel.MakeGuess("One", "Два");

            // Assert
            Assert.False(result);
            Assert.Equal(0, viewModel.CorrectAttempts);
            Assert.Equal(1, viewModel.TotalAttempts);
            Assert.Empty(viewModel.Matches);
        }

        /// <summary>
        /// MakeGuess method should throw an exception if a word is already matched.
        /// </summary>
        [Fact]
        public void MakeGuess_ShouldThrowException_WhenWordAlreadyMatched()
        {
            // Arrange
            var viewModel = new TestViewModel
            {
                Cards = new List<Card>
                {
                    new Card { WordEn = "One", WordUa = "Один" },
                },
            };

            // Act
            viewModel.MakeGuess("One", "Один");

            // Assert
            Assert.Throws<InvalidOperationException>(() => viewModel.MakeGuess("One", "Один"));
        }
    }
}
