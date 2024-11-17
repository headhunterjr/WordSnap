using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSnapWPFApp.DAL.Models
{
    internal interface IWordSnapRepository
    {
        public Task<User?> GetUserByEmail(string email);
        public Task<bool> UserExistsByEmailOrUsernameAsync(string username, string email);
        public Task<int> AddUserAsync(User user);
        public Task<IEnumerable<Cardset>> GetUsersCardsetsLibraryAsync(int userId);
        public Task<IEnumerable<Cardset>> GetUsersOwnCardsetsLibraryAsync(int userId);
        public Task<IEnumerable<Card>> GetCardsOfCardsetAsync(int cardsetId);
        public Task<int> AddCardsetAsync(Cardset cardset);
        public Task<int> AddCardAsync(Card card);
        public Task<IEnumerable<Cardset>> GetCardsetsFromSearchAsync(string searchQuery);
        public Task<IEnumerable<Cardset>> GetRandomCardsetsAsync();
        public Task<int> SwitchCardsetPrivacy(int cardsetId);
        public Task<int> DeleteCardFromCardset(int cardId);
        public Task<int> DeleteCardset(int cardsetId);
        public Task<int> AddTestProgressAsync(Progress progress);
        public Task<Progress?> GetProgress(int userId, int cardsetId);
        public Task<int> UpdateProgress(Progress progress);
        public Task<int> SaveChangesAsync();
        public Task<Cardset?> GetCardsetAsync(int cardsetId);
        public Task<int> UpdateCardsetAsync(Cardset cardset);
        public Task<int> UpdateCardAsync(Card card);
        public Task<int> AddCardsetToSavedLibraryAsync(Userscardset userscardset);
        public Task<Userscardset?> GetUserscardsetAsync(int userId, int cardsetId);
        public Task<bool> DeleteCardsetAsync(int cardsetId);
        public Task<bool> DeleteCardAsync(int cardId);
        public Task<bool> IsCardsetOwnedByUserAsync(int userId, int cardsetId);
        public Task<Card?> GetCardAsync(int cardId);
    }
}
