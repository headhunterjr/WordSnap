// <copyright file="IWordSnapRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.DAL.Models
{
    /// <summary>
    /// wordsnap repository interface.
    /// </summary>
    internal interface IWordSnapRepository
    {
        /// <summary>
        /// gets a user by email.
        /// </summary>
        /// <param name="email">email.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<User?> GetUserByEmail(string email);

        /// <summary>
        /// checks if a user exists by their username and email.
        /// </summary>
        /// <param name="username">username.</param>
        /// <param name="email">email.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<bool> UserExistsByEmailOrUsernameAsync(string username, string email);

        /// <summary>
        /// adds a user to the database.
        /// </summary>
        /// <param name="user">user.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<int> AddUserAsync(User user);

        /// <summary>
        /// gets a user's accessed cardset library.
        /// </summary>
        /// <param name="userId">user's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<IEnumerable<Cardset>> GetUsersCardsetsLibraryAsync(int userId);

        /// <summary>
        /// gets a user's owned cardset library.
        /// </summary>
        /// <param name="userId">user's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<IEnumerable<Cardset>> GetUsersOwnCardsetsLibraryAsync(int userId);

        /// <summary>
        /// gets cards of a cardset.
        /// </summary>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<IEnumerable<Card>> GetCardsOfCardsetAsync(int cardsetId);

        /// <summary>
        /// adds a cardset to the database.
        /// </summary>
        /// <param name="cardset">cardset.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<int> AddCardsetAsync(Cardset cardset);

        /// <summary>
        /// adds a card to the database.
        /// </summary>
        /// <param name="card">card.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<int> AddCardAsync(Card card);

        /// <summary>
        /// gets cardsets based on the search query.
        /// </summary>
        /// <param name="searchQuery">search query.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<IEnumerable<Cardset>> GetCardsetsFromSearchAsync(string searchQuery);

        /// <summary>
        /// gets random cardsets.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<IEnumerable<Cardset>> GetRandomCardsetsAsync();

        /// <summary>
        /// switches a cardet's privacy field.
        /// </summary>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<int> SwitchCardsetPrivacy(int cardsetId);

        /// <summary>
        /// deletes a card from the database.
        /// </summary>
        /// <param name="cardId">card's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<int> DeleteCardFromCardset(int cardId);

        /// <summary>
        /// deletes a cardset.
        /// </summary>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<int> DeleteCardset(int cardsetId);

        /// <summary>
        /// adds a test progress record.
        /// </summary>
        /// <param name="progress">progress.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<int> AddTestProgressAsync(Progress progress);

        /// <summary>
        /// gets a progress record.
        /// </summary>
        /// <param name="userId">user's Id.</param>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<Progress?> GetProgress(int userId, int cardsetId);

        /// <summary>
        /// updates a progress record.
        /// </summary>
        /// <param name="progress">progress.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<int> UpdateProgress(Progress progress);

        /// <summary>
        /// saves the changes made to the database.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<int> SaveChangesAsync();

        /// <summary>
        /// gets a cardset.
        /// </summary>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<Cardset?> GetCardsetAsync(int cardsetId);

        /// <summary>
        /// updates a cardset.
        /// </summary>
        /// <param name="cardset">cardset.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<int> UpdateCardsetAsync(Cardset cardset);

        /// <summary>
        /// updates a card.
        /// </summary>
        /// <param name="card">card.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<int> UpdateCardAsync(Card card);

        /// <summary>
        /// adds a cardset to the saved library.
        /// </summary>
        /// <param name="userscardset">userscardset.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<int> AddCardsetToSavedLibraryAsync(Userscardset userscardset);

        /// <summary>
        /// gets a userscardset record.
        /// </summary>
        /// <param name="userId">user's Id.</param>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<Userscardset?> GetUserscardsetAsync(int userId, int cardsetId);

        /// <summary>
        /// delets a cardset.
        /// </summary>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<bool> DeleteCardsetAsync(int cardsetId);

        /// <summary>
        /// deletes a card.
        /// </summary>
        /// <param name="cardId">card's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<bool> DeleteCardAsync(int cardId);

        /// <summary>
        /// checks if a cardset is owned by given user.
        /// </summary>
        /// <param name="userId">user's Id.</param>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<bool> IsCardsetOwnedByUserAsync(int userId, int cardsetId);

        /// <summary>
        /// gets a card.
        /// </summary>
        /// <param name="cardId">card's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<Card?> GetCardAsync(int cardId);

        /// <summary>
        /// deletes a userscardset record.
        /// </summary>
        /// <param name="userscardsetId">userscardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<bool> DeleteUsersCardset(int userscardsetId);
    }
}
