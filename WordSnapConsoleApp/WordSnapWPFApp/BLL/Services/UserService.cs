// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.BLL.Services
{
    using WordSnapWPFApp.DAL.Models;

    /// <summary>
    /// user service.
    /// </summary>
    internal class UserService : IDisposable
    {
        private static UserService? instance;

        private readonly WordSnapRepository repository = new WordSnapRepository();

        private User? loggedInUser;

        private bool disposed = false;

        private UserService()
        {
        }

        /// <summary>
        /// Gets instance.
        /// </summary>
        public static UserService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserService();
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a user is logged in.
        /// </summary>
        public bool IsUserLoggedIn => this.loggedInUser != null;

        /// <summary>
        /// logs the user in.
        /// </summary>
        /// <param name="email">email.</param>
        /// <param name="password">password.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<User> LoginUserAsync(string email, string password)
        {
            var user = await this.repository.GetUserByEmail(email);
            if (user == null)
            {
                throw new InvalidOperationException("Неправильна пошта.");
            }

            var hashedPassword = PasswordService.HashPassword(password, user.PasswordSalt);
            if (hashedPassword != user.PasswordHash)
            {
                throw new UnauthorizedAccessException("Неправильний пароль.");
            }

            this.loggedInUser = user;
            return this.loggedInUser;
        }

        /// <summary>
        /// registers a user.
        /// </summary>
        /// <param name="username">username.</param>
        /// <param name="email">email.</param>
        /// <param name="password">password.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task RegisterUserAsync(string username, string email, string password)
        {
            if (await this.repository.UserExistsByEmailOrUsernameAsync(username, email))
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
                CreatedAt = DateTime.Now,
            };
            await this.repository.AddUserAsync(user);
        }

        /// <summary>
        /// gets a logged in user.
        /// </summary>
        /// <returns>a user if one is logged in.</returns>
        public User? GetLoggedInUser()
        {
            return this.loggedInUser;
        }

        /// <summary>
        /// logs the user out.
        /// </summary>
        public void Logout()
        {
            this.loggedInUser = null;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!this.disposed)
            {
                this.repository?.Dispose();
                this.disposed = true;
            }
        }
    }
}
