// <copyright file="WordSnapRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.DAL.Models
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// wordsnap repository implementation.
    /// </summary>
    internal class WordSnapRepository : IWordSnapRepository, IDisposable
    {
        private readonly WordsnapdbContext context = new WordsnapdbContext();

        /// <inheritdoc/>
        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        /// <inheritdoc/>
        public async Task<bool> UserExistsByEmailOrUsernameAsync(string username, string email)
        {
            bool userExists = await this.context.Users.AnyAsync(u => u.Username == username || u.Email == email);
            return userExists;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.context.Dispose();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Cardset>> GetUsersCardsetsLibraryAsync(int userId)
        {
            var usersCardsetsIds = await this.context.Userscardsets.Where(uc => uc.UserRef == userId).Select(uc => uc.CardsetRef).ToListAsync();
            var usersCardsetsLibrary = await this.context.Cardsets.Where(c => usersCardsetsIds.Contains(c.Id)).ToListAsync();
            return usersCardsetsLibrary;
        }

        /// <inheritdoc/>
        public async Task<int> AddUserAsync(User user)
        {
            await this.context.Users.AddAsync(user);
            return await this.context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> AddCardsetAsync(Cardset cardset)
        {
            await this.context.Cardsets.AddAsync(cardset);
            return await this.context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> AddCardAsync(Card card)
        {
            await this.context.Cards.AddAsync(card);
            return await this.context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Card>> GetCardsOfCardsetAsync(int cardsetId)
        {
            var cards = await this.context.Cards.Where(c => c.CardsetRef == cardsetId).ToListAsync();
            return cards;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Cardset>> GetCardsetsFromSearchAsync(string searchQuery)
        {
            var cardsets = await this.context.Cardsets.Where(cs => cs.Name.ToLower().Contains(searchQuery.ToLower())).Where(cs => cs.IsPublic ?? false).ToListAsync();
            return cardsets;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Cardset>> GetRandomCardsetsAsync()
        {
            var cardsets = await this.context.Cardsets.OrderBy(cs => Guid.NewGuid()).Where(cs => cs.IsPublic ?? false).ToListAsync();
            return cardsets;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Cardset>> GetUsersOwnCardsetsLibraryAsync(int userId)
        {
            var cardsets = await this.context.Cardsets.Where(cs => cs.UserRef == userId).ToListAsync();
            return cardsets;
        }

        /// <inheritdoc/>
        public async Task<int> SwitchCardsetPrivacy(int cardsetId)
        {
            var cardset = await this.context.Cardsets.FirstOrDefaultAsync(cs => cs.Id == cardsetId);
            if (cardset == null)
            {
                throw new InvalidOperationException("Колекцію не знайдено.");
            }

            cardset.IsPublic = !cardset.IsPublic;
            return await this.context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> DeleteCardFromCardset(int cardId)
        {
            var card = await this.context.Cards.FirstOrDefaultAsync(c => c.Id == cardId);
            if (card == null)
            {
                throw new InvalidOperationException("Картку не знайдено.");
            }

            this.context.Cards.Remove(card);
            return await this.context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> DeleteCardset(int cardsetId)
        {
            var cardset = await this.context.Cardsets.FirstOrDefaultAsync(cs => cs.Id == cardsetId);
            if (cardset == null)
            {
                throw new InvalidOperationException("Колекцію не знайдено.");
            }

            this.context.Cardsets.Remove(cardset);
            return await this.context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> AddTestProgressAsync(Progress progress)
        {
            this.context.Progresses.Add(progress);
            return await this.context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<Progress?> GetProgress(int userId, int cardsetId)
        {
            var progress = await this.context.Progresses.FirstOrDefaultAsync(p => p.UserRef == userId & p.CardsetRef == cardsetId);
            return progress;
        }

        /// <inheritdoc/>
        public async Task<int> UpdateProgress(Progress progress)
        {
            this.context.Progresses.Update(progress);
            return await this.context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<Cardset?> GetCardsetAsync(int cardsetId)
        {
            var cardset = await this.context.Cardsets.FirstOrDefaultAsync(cs => cs.Id == cardsetId);
            return cardset;
        }

        /// <inheritdoc/>
        public async Task<int> UpdateCardsetAsync(Cardset cardset)
        {
            this.context.Cardsets.Update(cardset);
            return await this.context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> UpdateCardAsync(Card card)
        {
            this.context.Cards.Update(card);
            return await this.context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> AddCardsetToSavedLibraryAsync(Userscardset userscardset)
        {
            this.context.Userscardsets.Add(userscardset);
            return await this.context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<Userscardset?> GetUserscardsetAsync(int userId, int cardsetId)
        {
            var userscardset = await this.context.Userscardsets.FirstOrDefaultAsync(uc => uc.UserRef == userId && uc.CardsetRef == cardsetId);
            return userscardset;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteCardsetAsync(int cardsetId)
        {
            var cardset = await this.GetCardsetAsync(cardsetId);

            if (cardset != null)
            {
                this.context.Cardsets.Remove(cardset);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteCardAsync(int cardId)
        {
            var card = await this.GetCardAsync(cardId);

            if (card != null)
            {
                this.context.Cards.Remove(card);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> IsCardsetOwnedByUserAsync(int userId, int cardsetId)
        {
            var cardset = await this.GetCardsetAsync(cardsetId);
            if (cardset == null)
            {
                throw new InvalidOperationException("Колекцію не знайдено");
            }

            return cardset.UserRef == userId;
        }

        /// <inheritdoc/>
        public async Task<Card?> GetCardAsync(int cardId)
        {
            var card = await this.context.Cards.FirstOrDefaultAsync(c => c.Id == cardId);
            return card;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteUsersCardset(int userscardsetId)
        {
            var userscardset = await this.context.Userscardsets.FirstOrDefaultAsync(uc => uc.Id == userscardsetId);
            if (userscardset != null)
            {
                this.context.Userscardsets.Remove(userscardset);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
