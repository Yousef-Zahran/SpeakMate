using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
namespace SpeakMate.Infrastructure.Services
{

    public class DiffService
    {
        private readonly Differ _differ = new Differ();

        public (int start, int end) FindWrongPart(string original, string corrected)
        {
            return FindRange(original, corrected, original);
        }

        public (int start, int end) FindCorrectPart(string original, string corrected)
        {
            return FindRange(original, corrected, corrected);
        }

        private (int start, int end) FindRange(string original, string corrected, string source)
        {
            var origWords = original.Split(' ');
            var correctWords = corrected.Split(' ');

            var isOriginal = source == original;
            var sourceWords = isOriginal ? origWords : correctWords;

            int start = -1;
            int end = -1;
            int charIndex = 0;

            int minLen = Math.Min(origWords.Length, correctWords.Length);

            for (int i = 0; i < Math.Max(origWords.Length, correctWords.Length); i++)
            {
                bool changed = i >= minLen || origWords[i] != correctWords[i];

                if (changed && i < sourceWords.Length)
                {
                    if (start == -1)
                        start = charIndex;

                    end = charIndex + sourceWords[i].Length;
                }

                if (i < sourceWords.Length)
                    charIndex += sourceWords[i].Length + 1; // +1 for space
            }

            return (start == -1 ? 0 : start,
                    end == -1 ? 0 : end);
        }
    }
}