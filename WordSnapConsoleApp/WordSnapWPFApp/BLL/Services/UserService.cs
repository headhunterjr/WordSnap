using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordSnapWPFApp.DAL.Models;

namespace WordSnapWPFApp.BLL.Services
{
    class UserService :IDisposable
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

        public async Task<User> LoginUserAsync(string email, string password)
        {
            var user = await _repository.GetUserByEmail(email);
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
            if (await _repository.UserExistsByEmailOrUsernameAsync(username, email))
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
                CreatedAt = DateTime.Now
            };
            await _repository.AddUserAsync(user);
        }
    }
}
