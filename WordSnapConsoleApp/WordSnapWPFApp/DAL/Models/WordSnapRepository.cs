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
    }

}
