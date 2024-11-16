using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordSnapWPFApp.BLL.Services;

namespace WordSnapWPFApp.DAL.Models
{
    internal class WordSnapRepository : IWordSnapRepository, IDisposable
    {
        private readonly WordsnapdbContext _context = new WordsnapdbContext();

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<bool> UserExistsByEmailOrUsernameAsync(string username, string email)
        {
            bool userExists = await _context.Users.AnyAsync(u => u.Username == username || u.Email == email);
            return userExists;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<Cardset>> GetUsersCardsetsLibraryAsync(int userId)
        {
            var usersCardsetsIds = await _context.Userscardsets.Where(uc => uc.UserRef == userId).Select(uc => uc.CardsetRef).ToListAsync();
            var usersCardsetsLibrary = await _context.Cardsets.Where(c => usersCardsetsIds.Contains(c.Id)).ToListAsync();
            return usersCardsetsLibrary;
        }

        public async Task<int> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddCardsetAsync(Cardset cardset)
        {
            await _context.Cardsets.AddAsync(cardset);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddCardAsync(Card card)
        {
            await _context.Cards.AddAsync(card);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Card>> GetCardsOfCardsetAsync(int cardsetId)
        {
            var cards = await _context.Cards.Where(c => c.CardsetRef == cardsetId).ToListAsync();
            return cards;
        }

        public async Task<IEnumerable<Cardset>> GetCardsetsFromSearchAsync(string searchQuery)
        {
            var cardsets = await _context.Cardsets.Where(cs => cs.Name.ToLower().Contains(searchQuery.ToLower())).Where(cs => cs.IsPublic ?? false).ToListAsync();
            return cardsets;
        }

        public async Task<IEnumerable<Cardset>> GetRandomCardsetsAsync()
        {
            var cardsets = await _context.Cardsets.OrderBy(cs => Guid.NewGuid()).Where(cs => cs.IsPublic ?? false).ToListAsync();
            return cardsets;
        }

        public async Task<IEnumerable<Cardset>> GetUsersOwnCardsetsLibraryAsync(int userId)
        {
            var cardsets = await _context.Cardsets.Where(cs => cs.UserRef == userId).ToListAsync();
            return cardsets;
        }

        public async Task<int> SwitchCardsetPrivacy(int cardsetId)
        {
            var cardset = await _context.Cardsets.FirstOrDefaultAsync(cs => cs.Id == cardsetId);
            if (cardset == null)
            {
                throw new InvalidOperationException("Кардсет не знайдено.");
            }
            cardset.IsPublic = !cardset.IsPublic;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteCardFromCardset(int cardId)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == cardId);
            if (card == null)
            {
                throw new InvalidOperationException("Картку не знайдено.");
            }
            _context.Cards.Remove(card);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteCardset(int cardsetId)
        {
            var cardset = await _context.Cardsets.FirstOrDefaultAsync(cs => cs.Id == cardsetId);
            if (cardset == null)
            {
                throw new InvalidOperationException("Кардсет не знайдено.");
            }
            _context.Cardsets.Remove(cardset);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddTestProgressAsync(Progress progress)
        {
            _context.Progresses.Add(progress);
            return await _context.SaveChangesAsync();
        }

        public async Task<Progress?> GetProgress(int userId, int cardsetId)
        {
            var progress = await _context.Progresses.FirstOrDefaultAsync(p => p.UserRef == userId & p.CardsetRef == cardsetId);
            return progress;
        }
        public async Task<int> UpdateProgress(Progress progress)
        {
            _context.Progresses.Update(progress);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Cardset?> GetCardsetAsync(int cardsetId)
        {
            var cardset = await _context.Cardsets.FirstOrDefaultAsync(cs => cs.Id == cardsetId);
            return cardset;
        }
        public async Task<int> UpdateCardsetAsync(Cardset cardset)
        {
            _context.Cardsets.Update(cardset);
            return await _context.SaveChangesAsync();
        }
    }

}
