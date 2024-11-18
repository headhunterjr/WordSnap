// <copyright file="CardsetService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.BLL.Services
{
    using WordSnapWPFApp.DAL.Models;

    /// <summary>
    /// cardset service.
    /// </summary>
    internal class CardsetService : IDisposable
    {
        private readonly WordSnapRepository repository = new WordSnapRepository();
        private bool disposed = false;

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!this.disposed)
            {
                this.repository?.Dispose();
                this.disposed = true;
            }
        }

        /// <summary>
        /// gets cardsets from a search query.
        /// </summary>
        /// <param name="searchQuery">search query.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IEnumerable<Cardset>> GetCardsetsFromSearchAsync(string searchQuery)
        {
            var cardsets = await this.repository.GetCardsetsFromSearchAsync(searchQuery);
            return cardsets.Take(3);
        }

        /// <summary>
        /// gets 3 random cardsets for the main page.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IEnumerable<Cardset>> GetRandomCardsetsAsync()
        {
            var cardsets = await this.repository.GetRandomCardsetsAsync();
            return cardsets.Take(3);
        }

        /// <summary>
        /// gets all the cardsets that a user owns.
        /// </summary>
        /// <param name="userId">user's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IEnumerable<Cardset>> GetUsersOwnCardsetsLibraryAsync(int userId)
        {
            var cardsets = await this.repository.GetUsersOwnCardsetsLibraryAsync(userId);
            return cardsets;
        }

        /// <summary>
        /// gets a user's library of cardsets.
        /// </summary>
        /// <param name="userId">user's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IEnumerable<Cardset>> GetUsersCardsetsLibraryAsync(int userId)
        {
            var cardsets = await this.repository.GetUsersCardsetsLibraryAsync(userId);
            return cardsets;
        }

        /// <summary>
        /// gets all the cards in a cardset.
        /// </summary>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IEnumerable<Card>> GetCardsOfCardsetAsync(int cardsetId)
        {
            var cards = await this.repository.GetCardsOfCardsetAsync(cardsetId);
            return cards;
        }

        /// <summary>
        /// gets 5 random cards of a cardset.
        /// </summary>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IEnumerable<Card>> GetCardsOfCardsetForTestAsync(int cardsetId)
        {
            var allCards = await this.repository.GetCardsOfCardsetAsync(cardsetId);
            var testCards = allCards.OrderBy(_ => Guid.NewGuid()).Take(5);
            return testCards;
        }

        /// <summary>
        /// saves test progress.
        /// </summary>
        /// <param name="userId">user's Id.</param>
        /// <param name="cardsetId">cardsets's Id.</param>
        /// <param name="successRate">test success rate.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<int> SaveTestProgressAsync(int userId, int cardsetId, double successRate)
        {
            var existingProgress = await this.repository.GetProgress(userId, cardsetId);
            if (existingProgress == null)
            {
                var progress = new Progress
                {
                    UserRef = userId,
                    CardsetRef = cardsetId,
                    LastAccessed = DateTime.Now,
                    SuccessRate = successRate,
                };
                return await this.repository.AddTestProgressAsync(progress);
            }

            if (successRate > existingProgress.SuccessRate)
            {
                existingProgress.SuccessRate = successRate;
                return await this.repository.UpdateProgress(existingProgress);
            }

            return await this.repository.SaveChangesAsync();
        }

        /// <summary>
        /// gets a cardset.
        /// </summary>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<Cardset> GetCardsetAsync(int cardsetId)
        {
            var cardset = await this.repository.GetCardsetAsync(cardsetId);
            if (cardset == null)
            {
                throw new InvalidOperationException("Ця колекція не існує");
            }

            return cardset;
        }

        /// <summary>
        /// updates a cardset's name.
        /// </summary>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <param name="name">cardset's name.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<int> UpdateCardsetNameAsync(int cardsetId, string name)
        {
            var cardset = await this.GetCardsetAsync(cardsetId);
            cardset.Name = name;
            return await this.repository.UpdateCardsetAsync(cardset);
        }

        /// <summary>
        /// adds a cardset to the database.
        /// </summary>
        /// <param name="cardset">cardset.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<int> CreateCardsetAsync(Cardset cardset)
        {
            return await this.repository.AddCardsetAsync(cardset);
        }

        /// <summary>
        /// updates a cardset.
        /// </summary>
        /// <param name="cardset">cardset.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<int> UpdateCardsetAsync(Cardset cardset)
        {
            return await this.repository.UpdateCardsetAsync(cardset);
        }

        /// <summary>
        /// adds a card to a cardset.
        /// </summary>
        /// <param name="card">card.</param>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<int> AddCardToCardsetAsync(Card card, int cardsetId)
        {
            card.CardsetRef = cardsetId;
            return await this.repository.AddCardAsync(card);
        }

        /// <summary>
        /// updates a card.
        /// </summary>
        /// <param name="card">card.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<int> UpdateCardAsync(Card card)
        {
            return await this.repository.UpdateCardAsync(card);
        }

        /// <summary>
        /// creates or updates a cardset.
        /// </summary>
        /// <param name="cardset">cardset.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<int> CreateOrUpdateCardsetAsync(Cardset cardset)
        {
            if (cardset.Id == 0)
            {
                return await this.repository.AddCardsetAsync(cardset);
            }
            else
            {
                return await this.repository.UpdateCardsetAsync(cardset);
            }
        }

        /// <summary>
        /// creates or updates a card.
        /// </summary>
        /// <param name="card">card.</param>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<int> CreateOrUpdateCardAsync(Card card, int? cardsetId)
        {
            if (cardsetId.HasValue)
            {
                card.CardsetRef = cardsetId.Value;
            }

            if (card.Id == 0)
            {
                return await this.repository.AddCardAsync(card);
            }
            else
            {
                return await this.repository.UpdateCardAsync(card);
            }
        }

        /// <summary>
        /// adds a cardset to the user's saved library.
        /// </summary>
        /// <param name="userId">user's Id.</param>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<int> AddCardsetToSavedLibraryAsync(int userId, int cardsetId)
        {
            var existingUserscardset = await this.repository.GetUserscardsetAsync(userId, cardsetId);
            if (existingUserscardset != null)
            {
                throw new InvalidOperationException($"Колекція вже є серед збережених.");
            }

            Userscardset userscardset = new Userscardset
            {
                UserRef = userId,
                CardsetRef = cardsetId,
            };
            return await this.repository.AddCardsetToSavedLibraryAsync(userscardset);
        }

        /// <summary>
        /// gets a user's cardset record.
        /// </summary>
        /// <param name="userId">user's Id.</param>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<Userscardset?> GetUserscardsetAsync(int userId, int cardsetId)
        {
            var userscardset = await this.repository.GetUserscardsetAsync(userId, cardsetId);
            if (userscardset == null)
            {
                throw new InvalidOperationException("Ви не маєте доступу до цієї колекції");
            }

            return userscardset;
        }

        /// <summary>
        /// deletes a cardset.
        /// </summary>
        /// <param name="userId">user's Id.</param>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task DeleteCardsetAsync(int userId, int cardsetId)
        {
            var isOwner = await this.repository.IsCardsetOwnedByUserAsync(userId, cardsetId);
            if (!isOwner)
            {
                throw new InvalidOperationException($"Ви не є власником цієї колекції.");
            }

            var success = await this.repository.DeleteCardsetAsync(cardsetId);
            if (!success)
            {
                throw new InvalidOperationException($"Не вдалося видалити колекцію.");
            }
        }

        /// <summary>
        /// deletes a card.
        /// </summary>
        /// <param name="userId">user's Id.</param>
        /// <param name="cardId">card's Id.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task DeleteCardAsync(int userId, int cardId)
        {
            var card = await this.repository.GetCardAsync(cardId);
            if (card == null)
            {
                throw new InvalidOperationException($"Картку не знайдено.");
            }

            var isOwner = await this.repository.IsCardsetOwnedByUserAsync(userId, card.CardsetRef);
            if (!isOwner)
            {
                throw new InvalidOperationException($"Ви не є власником цієї колекції.");
            }

            var success = await this.repository.DeleteCardAsync(cardId);
            if (!success)
            {
                throw new InvalidOperationException($"Не вдалося видалити картку.");
            }
        }

        /// <summary>
        /// deletes a user's cardset record.
        /// </summary>
        /// <param name="userId">user's Id.</param>
        /// <param name="cardsetId">cardset's Id.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task DeleteUserscardsetAsync(int userId, int cardsetId)
        {
            var userscardset = await this.repository.GetUserscardsetAsync(userId, cardsetId);
            var user = UserService.Instance.GetLoggedInUser();
            if (userscardset == null)
            {
                throw new InvalidOperationException("Запис не знайдено.");
            }

            if (user != null && user.Id == userId)
            {
                var success = await this.repository.DeleteUsersCardset(userscardset.Id);
                if (!success)
                {
                    throw new InvalidOperationException("Не вдалося видалити запис");
                }
            }
        }
    }
}
