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

        public async Task<User> LoginUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid email.");
            }
            var hashedPassword = PasswordService.HashPassword(password, user.PasswordSalt);
            if (hashedPassword != user.PasswordHash)
            {
                throw new UnauthorizedAccessException("Invalid password.");
            }
            return user;
        }

        public async Task RegisterUserAsync(string username, string email, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username || u.Email == email))
            {
                throw new InvalidOperationException("User with given username or email already exists");
            }
            string salt = PasswordService.GenerateSalt();
            string passwordHash = PasswordService.HashPassword(password, salt);
            User user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = salt,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<Cardset>> GetUsersCardsetsLibraryAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Please log in first.");
            }
            var usersCardsetsIds = await _context.Userscardsets.Where(uc => uc.UserRef == userId).Select(uc => uc.CardsetRef).ToListAsync();
            var usersCardsetsLibrary = await _context.Cardsets.Where(c => usersCardsetsIds.Contains(c.Id)).ToListAsync();
            return usersCardsetsLibrary;
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
            if (cardset == null)
            {
                throw new InvalidOperationException("No such cardset.");
            }
            return await _context.Cards.Where(c => c.CardsetRef == cardsetId).ToListAsync();
        }
    }

}
