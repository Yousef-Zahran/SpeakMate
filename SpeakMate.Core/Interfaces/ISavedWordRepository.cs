using SpeakMate.Core.DTOs;
using SpeakMate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Interfaces
{
    public interface ISavedWordRepository
    {
        Task<SavedWord?> GetByIdAsync(string id);
        Task<IEnumerable<SavedWordDto>> GetSavedWordsAsync(string memberId);
        Task<SavedWordDto?> SaveWordAsync(string memberId, CreateSavedWordDto dto);
        Task<bool> DeleteWordAsync(string id, string memberId);
        Task<bool> SaveAllAsync();
    }
}
