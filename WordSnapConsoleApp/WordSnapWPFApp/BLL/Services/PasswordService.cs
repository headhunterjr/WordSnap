using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WordSnapWPFApp.BLL.Services
{
    internal class PasswordService
    {
        private const int saltSize = 16;
        public static string GenerateSalt()
        {
            var saltBytes = new byte[saltSize];
            RandomNumberGenerator.Fill(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
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
