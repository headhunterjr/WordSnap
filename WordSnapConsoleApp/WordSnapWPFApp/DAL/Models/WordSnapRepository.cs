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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
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
            var cardset = await _context.Cardsets.FirstOrDefaultAsync(cs => cs.Id == cardsetId);
            return await _context.Cards.Where(c => c.CardsetRef == cardsetId).ToListAsync();
        }

        public async Task<IEnumerable<Cardset>> GetCardsetsFromSearchAsync(string searchQuery)
        {
            var cardsets = await _context.Cardsets.Where(cs => cs.Name.ToLower().Contains(searchQuery.ToLower())).ToListAsync();
            return cardsets;
        }

        public async Task<IEnumerable<Cardset>> GetRandomCardsetsAsync()
        {
            var cardsets = await _context.Cardsets.OrderBy(cs => Guid.NewGuid()).ToListAsync();
            return cardsets;
        }
    }

}
