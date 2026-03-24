using SpeakMate.Core.DTOs;
using SpeakMate.Core.Entities;
using SpeakMate.Core.Pagination;
using SpeakMate.Core.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void RemoveMessage(Message message);
        Task<Message?>GetMessage(string MessageId);

        Task<PaginatedResult<MessageDto>> GetMessagesForMember(MessageParams messageParams);
        Task<IReadOnlyList<MessageDto>>GetMessageThread(string CurrentMemberId,string RecipientId);
        Task<bool> SaveAllAsync();
    }
}