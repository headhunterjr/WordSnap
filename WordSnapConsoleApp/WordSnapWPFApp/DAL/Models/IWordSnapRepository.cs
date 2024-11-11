﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSnapWPFApp.DAL.Models
{
    internal interface IWordSnapRepository
    {
        public Task<User?> GetUserByEmail(string email);
        public Task<bool> UserExistsByEmailOrUsernameAsync(string username, string email);
        public Task<int> AddUserAsync(User user);
        public Task<IEnumerable<Cardset>> GetUsersCardsetsLibraryAsync(int userId);
        public Task<IEnumerable<Card>> GetCardsOfCardsetAsync(int cardsetId);
        public Task<int> AddCardsetAsync(Cardset cardset);
        public Task<int> AddCardAsync(Card card);
        public Task<IEnumerable<Cardset>> GetCardsetsFromSearchAsync(string searchQuery);
        public Task<IEnumerable<Cardset>> GetRandomCardsetsAsync();
    }
}
