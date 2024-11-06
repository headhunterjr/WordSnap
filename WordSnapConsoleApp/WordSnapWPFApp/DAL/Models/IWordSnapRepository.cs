using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSnapWPFApp.DAL.Models
{
    internal interface IWordSnapRepository
    {
        public Task RegisterUserAsync(string username, string email, string password);
        public Task<User> LoginUserAsync(string email, string password);
        public Task<bool> SaveChangesAsync();
    }
}
