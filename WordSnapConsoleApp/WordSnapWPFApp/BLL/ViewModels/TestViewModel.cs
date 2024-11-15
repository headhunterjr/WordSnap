using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordSnapWPFApp.DAL.Models;

namespace WordSnapWPFApp.BLL.ViewModels
{
    internal class TestViewModel
    {
        public List<Card> Cards { get; set; }
        public Dictionary<string, string> Matches { get; set; } = new();
        public int TotalAttempts { get; private set; }
        public int CorrectAttempts { get; private set; }

        public double Accuracy => TotalAttempts > 0 ? (double)CorrectAttempts / TotalAttempts : 0.0;

        public bool MakeGuess(string wordEn, string wordUa)
        {
            TotalAttempts++;

            if (Matches.ContainsKey(wordEn))
            {
                throw new InvalidOperationException("WordEn already matched.");
            }

            if (Cards.First(c => c.WordEn == wordEn).WordUa == wordUa)
            {
                CorrectAttempts++;
                Matches[wordEn] = wordUa;
                return true;
            }
            return false;
        }

        public bool IsTestComplete => Matches.Count == Cards.Count;
    }
}
