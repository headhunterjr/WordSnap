// <copyright file="PasswordService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.BLL.Services
{
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// password service.
    /// </summary>
    public class PasswordService
    {
        private const int SaltSize = 16;

        /// <summary>
        /// generates salt.
        /// </summary>
        /// <returns>salt.</returns>
        public static string GenerateSalt()
        {
            var saltBytes = new byte[SaltSize];
            RandomNumberGenerator.Fill(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// hashes a password.
        /// </summary>
        /// <param name="password">password.</param>
        /// <param name="salt">salt.</param>
        /// <returns>hashed password.</returns>
        public static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var firstHashBytes = sha256.ComputeHash(passwordBytes);
                var firstHash = Convert.ToBase64String(firstHashBytes);

                var saltedHashBytes = Encoding.UTF8.GetBytes(firstHash + salt);
                var finalHashBytes = sha256.ComputeHash(saltedHashBytes);
                return Convert.ToBase64String(finalHashBytes);
            }
        }
    }
}
