﻿using Microsoft.EntityFrameworkCore;
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
        private static UserService? _instance;
        private User? _loggedInUser;
        private readonly WordSnapRepository _repository = new WordSnapRepository();
        private bool _disposed = false;

        public bool IsUserLoggedIn => _loggedInUser != null;

        private UserService() { }
        public static UserService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserService();
                }
                return _instance;
            }
        }

        public async Task<User> LoginUserAsync(string email, string password)
        {
            var user = await _repository.GetUserByEmail(email);
            if (user == null)
            {
                throw new InvalidOperationException("Неправильна пошта.");
            }
            var hashedPassword = PasswordService.HashPassword(password, user.PasswordSalt);
            if (hashedPassword != user.PasswordHash)
            {
                throw new UnauthorizedAccessException("Неправильний пароль.");
            }
            _loggedInUser = user;
            return _loggedInUser;
        }

        public async Task RegisterUserAsync(string username, string email, string password)
        {
            if (await _repository.UserExistsByEmailOrUsernameAsync(username, email))
            {
                throw new InvalidOperationException("Користувач з таким іменем або поштою вже існує.");
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

        public User? GetLoggedInUser()
        {
            return _loggedInUser;
        }

        public void Logout()
        {
            _loggedInUser = null;
        }
        public void Dispose()
        {
            if (!_disposed)
            {
                _repository?.Dispose();
                _disposed = true;
            }
        }
    }
}
