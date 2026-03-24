using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpeakMate.Infrastructure.Services
{
    public class LanguageDetectionService
    {
        private static readonly RegexOptions Options = RegexOptions.Compiled | RegexOptions.IgnoreCase;

        
        private static readonly Dictionary<string, string> ScriptMap = new()
        {
            { @"\p{IsArabic}", "Arabic" },
            { @"[\u4e00-\u9fff]", "Chinese" },
            { @"[\u3040-\u30ff]", "Japanese" },
            { @"[\uac00-\ud7af]", "Korean" },
            { @"[\u0400-\u04ff]", "Russian" },
            { @"[\u0370-\u03ff]", "Greek" },
            { @"[\u0900-\u097f]", "Hindi" }
        };

        
        private static readonly Dictionary<string, string[]> LanguageWords = new()
        {
            { "English", new[] { "the", "and", "with", "have", "this", "that", "hello" } },
            { "French", new[] { "les", "des", "pour", "dans", "elle", "nous", "merci" } },
            { "Spanish", new[] { "los", "las", "para", "como", "este", "todo", "gracias" } },
            { "German", new[] { "das", "ist", "und", "nicht", "mit", "eine", "danke" } }
        };

        public string Detect(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "Unknown";

            foreach (var script in ScriptMap)
            {
                if (Regex.IsMatch(text, script.Key, Options))
                    return script.Value;
            }

           
            var words = Regex.Matches(text.ToLower(), @"\b\w+\b")
                             .Cast<Match>()
                             .Select(m => m.Value)
                             .ToList();

            if (words.Count == 0) return "Unknown";

            var scores = new Dictionary<string, int>();

            foreach (var lang in LanguageWords)
            {
                scores[lang.Key] = words.Count(w => lang.Value.Contains(w));
            }

            var bestMatch = scores.OrderByDescending(x => x.Value).First();

            return bestMatch.Value > 0 ? bestMatch.Key : "English";
        }
    }
}