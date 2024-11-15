using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
