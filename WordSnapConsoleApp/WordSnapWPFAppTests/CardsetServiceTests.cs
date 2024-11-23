// <copyright file="CardsetServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFAppTests
{
    using WordSnapWPFApp.BLL.Services;

    /// <summary>
    /// Cardset service tests.
    /// </summary>
    public class CardsetServiceTests
    {
        /// <summary>
        /// GetCardsOfCardsetForTestAsync should return 5 cards at most.
        /// </summary>
        [Fact]
        public async void GetCardsOfCardsetForTestAsync_ReturnsAtMostFiveCards()
        {
            // Arrange
            var service = new CardsetService();
            var cardsetId = 2408;

            // Act
            var result = await service.GetCardsOfCardsetForTestAsync(cardsetId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count() <= 5);
        }

        /// <summary>
        /// GetCardsetAsync should throw an exception if the cardset does not exist.
        /// </summary>
        [Fact]
        public async void GetCardsetAsync_ThrowsException_IfCardsetDoesNotExist()
        {
            // Arrange
            var service = new CardsetService();
            var cardsetId = 999;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.GetCardsetAsync(cardsetId));
        }

        /// <summary>
        /// DeleteCardsetAsync should throw exception if the user is not the owner.
        /// </summary>
        [Fact]
        public async void DeleteCardsetAsync_ThrowsException_IfUserIsNotOwner()
        {
            // Arrange
            var service = new CardsetService();
            var userId = 2;
            var cardsetId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.DeleteCardsetAsync(userId, cardsetId));
        }

        /// <summary>
        /// AddCardsetToSavedLibraryAsync should throw exception if the cardset is already saved for the user.
        /// </summary>
        [Fact]
        public async void AddCardsetToSavedLibraryAsync_ThrowsException_IfDuplicate()
        {
            // Arrange
            var service = new CardsetService();
            var userId = 1;
            var cardsetId = 2;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.AddCardsetToSavedLibraryAsync(userId, cardsetId));
        }

        /// <summary>
        /// IsCardsetInUserSavedLibraryAsync should return true if the cardset is saved for the user.
        /// </summary>
        [Fact]
        public async void IsCardsetInUserSavedLibraryAsync_ReturnsTrue_IfCardsetExists()
        {
            // Arrange
            var service = new CardsetService();
            var userId = 1;
            var cardsetId = 2;

            // Act
            var result = await service.IsCardsetInUserSavedLibraryAsync(userId, cardsetId);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// IsCardsetInUserSavedLibraryAsync should return false if the cardset is not saved for the user.
        /// </summary>
        [Fact]
        public async void IsCardsetInUserSavedLibraryAsync_ReturnsFalse_IfCardsetDoesNotExist()
        {
            // Arrange
            var service = new CardsetService();
            var userId = 1;
            var cardsetId = 999;

            // Act
            var result = await service.IsCardsetInUserSavedLibraryAsync(userId, cardsetId);

            // Assert
            Assert.False(result);
        }
    }
}
