// <copyright file="TestViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFApp.BLL.ViewModels
{
    using WordSnapWPFApp.DAL.Models;

    /// <summary>
    /// test view model.
    /// </summary>
    public class TestViewModel
    {
        /// <summary>
        /// Gets or sets cards.
        /// </summary>
        public List<Card> Cards { get; set; }

        /// <summary>
        /// Gets or sets matches.
        /// </summary>
        public Dictionary<string, string> Matches { get; set; } = new ();

        /// <summary>
        /// Gets total attempts.
        /// </summary>
        public int TotalAttempts { get; private set; }

        /// <summary>
        /// Gets correct attempts.
        /// </summary>
        public int CorrectAttempts { get; private set; }

        /// <summary>
        /// Gets accuracy.
        /// </summary>
        public double Accuracy => this.TotalAttempts > 0 ? (double)this.CorrectAttempts / this.TotalAttempts : 0.0;

        /// <summary>
        /// Gets a value indicating whether a test is complete.
        /// </summary>
        public bool IsTestComplete => this.Matches.Count == this.Cards.Count;

        /// <summary>
        /// makes a guess.
        /// </summary>
        /// <param name="wordEn">word in english.</param>
        /// <param name="wordUa">word in ukrainian.</param>
        /// <returns>whether a guess was right.</returns>
        /// <exception cref="InvalidOperationException">if wordEn was already matched.</exception>
        public bool MakeGuess(string wordEn, string wordUa)
        {
            this.TotalAttempts++;

            if (this.Matches.ContainsKey(wordEn))
            {
                throw new InvalidOperationException("WordEn already matched.");
            }

            if (this.Cards.First(c => c.WordEn == wordEn).WordUa == wordUa)
            {
                this.CorrectAttempts++;
                this.Matches[wordEn] = wordUa;
                return true;
            }

            return false;
        }
    }
}
