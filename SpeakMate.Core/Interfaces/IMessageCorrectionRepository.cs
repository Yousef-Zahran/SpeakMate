using SpeakMate.Core.DTOs;
using SpeakMate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Interfaces
{
    public interface IMessageCorrectionRepository
    {
        
        Task<MessageCorrectionDto?> AddCorrectionAsync(string correctorUserId, CreateMessageCorrectionDto dto);
        Task<bool> AcceptCorrectionAsync(string correctionId, string userId);
        Task<MessageCorrection?> GetByIdAsync(string id);
        Task<IEnumerable<MessageCorrection>> GetCorrectionsForMessageAsync(string messageId);
        Task<bool> SaveAllAsync();
    }
}
