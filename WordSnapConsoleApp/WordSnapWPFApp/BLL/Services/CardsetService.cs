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
    }
}
