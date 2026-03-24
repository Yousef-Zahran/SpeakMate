using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Interfaces
{
    public interface ITranslationService
    {
        Task<string> TranslateAsync(string text, string fromLanguage, string toLanguage);
    }
}
