using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using WordSnapWPFApp.DAL.Models;

namespace WordSnapWPFApp.BLL.Services
{
    class CardsetService :IDisposable
    {
        private readonly WordSnapRepository _repository = new WordSnapRepository();
        private bool _disposed = false;

        public void Dispose()
        {
            if (!_disposed)
            {
                _repository?.Dispose();
                _disposed = true;
            }
        }

        public async Task<IEnumerable<Cardset>> GetCardsetsFromSearchAsync(string searchQuery)
        {
            var cardsets = await _repository.GetCardsetsFromSearchAsync(searchQuery);
            return cardsets.Take(3);
        }
        public async Task<IEnumerable<Cardset>> GetRandomCardsetsAsync()
        {
            var cardsets = await _repository.GetRandomCardsetsAsync();
            return cardsets.Take(3);
        }
        public async Task<IEnumerable<Cardset>> GetUsersOwnCardsetsLibraryAsync(int userId)
        {
            var cardsets = await _repository.GetUsersOwnCardsetsLibraryAsync(userId);
            return cardsets;
        }
        public async Task<IEnumerable<Cardset>> GetUsersCardsetsLibraryAsync(int userId)
        {
            var cardsets = await _repository.GetUsersCardsetsLibraryAsync(userId);
            return cardsets;
        }
        public async Task<IEnumerable<Card>> GetCardsOfCardsetAsync(int cardsetId)
        {
            var cards = await _repository.GetCardsOfCardsetAsync(cardsetId);
            return cards;
        }
        public async Task<IEnumerable<Card>> GetCardsOfCardsetForTestAsync(int cardsetId)
        {
            var allCards = await _repository.GetCardsOfCardsetAsync(cardsetId);
            var testCards = allCards.OrderBy(_ => Guid.NewGuid()).Take(5);
            return testCards;
        }
        public async Task<int> SaveTestProgressAsync(int userId, int cardsetId, double successRate)
        {
            var existingProgress = await _repository.GetProgress(userId, cardsetId);
            if (existingProgress == null)
            {
                var progress = new Progress
                {
                    UserRef = userId,
                    CardsetRef = cardsetId,
                    LastAccessed = DateTime.Now,
                    SuccessRate = successRate,
                };
                return await _repository.AddTestProgressAsync(progress);
            }
            if (successRate > existingProgress.SuccessRate)
            {
                existingProgress.SuccessRate = successRate;
                return await _repository.UpdateProgress(existingProgress);
            }
            return await _repository.SaveChangesAsync();
        }
        public async Task<Cardset> GetCardsetAsync(int cardsetId)
        {
            var cardset = await _repository.GetCardsetAsync(cardsetId);
            if (cardset == null)
            {
                throw new InvalidOperationException("Ця колекція не існує");
            }
            return cardset;
        }
        public async Task<int> UpdateCardsetNameAsync(int cardsetId, string name)
        {
            var cardset = await GetCardsetAsync(cardsetId);
            cardset.Name = name;
            return await _repository.UpdateCardsetAsync(cardset);
        }
        public async Task<int> CreateCardsetAsync(Cardset cardset)
        {
            return await _repository.AddCardsetAsync(cardset);
        }

        public async Task<int> UpdateCardsetAsync(Cardset cardset)
        {
            return await _repository.UpdateCardsetAsync(cardset);
        }

        public async Task<int> AddCardToCardsetAsync(Card card, int cardsetId)
        {
            card.CardsetRef = cardsetId;
            return await _repository.AddCardAsync(card);
        }

        public async Task<int> UpdateCardAsync(Card card)
        {
            return await _repository.UpdateCardAsync(card);
        }
        public async Task<int> CreateOrUpdateCardsetAsync(Cardset cardset)
        {
            if (cardset.Id == 0)
            {
                return await _repository.AddCardsetAsync(cardset);
            }
            else
            {
                return await _repository.UpdateCardsetAsync(cardset);
            }
        }

        public async Task<int> CreateOrUpdateCardAsync(Card card, int? cardsetId)
        {
            if (cardsetId.HasValue)
            {
                card.CardsetRef = cardsetId.Value;
            }

            if (card.Id == 0)
            {
                return await _repository.AddCardAsync(card);
            }
            else
            {
                return await _repository.UpdateCardAsync(card);
            }
        }
        public async Task<int> AddCardsetToSavedLibraryAsync(int userId, int cardsetId)
        {
            var existingUserscardset = await _repository.GetUserscardsetAsync(userId, cardsetId);
            if (existingUserscardset != null)
            {
                throw new InvalidOperationException($"Колекція вже є серед збережених.");
            }

            Userscardset userscardset = new Userscardset
            {
                UserRef = userId,
                CardsetRef = cardsetId,
            };
            return await _repository.AddCardsetToSavedLibraryAsync(userscardset);
        }

        public async Task<Userscardset?> GetUserscardsetAsync(int userId, int cardsetId)
        {
            var userscardset = await _repository.GetUserscardsetAsync(userId, cardsetId);
            return userscardset;
        }
        public async Task DeleteCardsetAsync(int userId, int cardsetId)
        {
            var isOwner = await _repository.IsCardsetOwnedByUserAsync(userId, cardsetId);
            if (!isOwner)
            {
                throw new InvalidOperationException($"Ви не є власником цієї колекції.");
            }

            var success = await _repository.DeleteCardsetAsync(cardsetId);
            if (!success)
            {
                throw new InvalidOperationException($"Не вдалося видалити колекцію.");
            }
        }

        public async Task DeleteCardAsync(int userId, int cardId)
        {
            var card = await _repository.GetCardAsync(cardId);
            if (card == null)
            {
                throw new InvalidOperationException($"Картку не знайдено.");
            }

            var isOwner = await _repository.IsCardsetOwnedByUserAsync(userId, card.CardsetRef);
            if (!isOwner)
            {
                throw new InvalidOperationException($"Ви не є власником цієї колекції.");
            }

            var success = await _repository.DeleteCardAsync(cardId);
            if (!success)
            {
                throw new InvalidOperationException($"Не вдалося видалити картку.");
            }
        }
        public async Task DeleteUserscardsetAsync(int userId, int cardsetId)
        {
            var userscardset = await _repository.GetUserscardsetAsync(userId, cardsetId);
            var user = UserService.Instance.GetLoggedInUser();
            if (userscardset == null)
            {
                throw new InvalidOperationException("Запис не знайдено.");
            }
            if (user != null && user.Id == userId)
            {
                var success = await _repository.DeleteUsersCardset(userscardset.Id);
                if (!success)
                {
                    throw new InvalidOperationException("Не вдалося видалити запис");
                }
            }

        }
    }
}
