using AutoMapper;
using AutoMapper.QueryableExtensions;
using SpeakMate.Core.DTOs;
using SpeakMate.Core.Entities;
using SpeakMate.Core.Interfaces;
using SpeakMate.Core.Pagination;
using SpeakMate.Core.Params;
using SpeakMate.Infrastructure.Data;
using SpeakMate.Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Infrastructure.Repositories
{
    public class MessageRepository(AppDbContext _Context,IMapper _mapper) : IMessageRepository
    {
        public void AddMessage(Message message)
        {
          _Context.Messages.Add(message);
        }

        public async Task<Message?> GetMessage(string messageId)
        {
            return await _Context.Messages
                  .Include(m => m.Sender) 
                  .Include(m => m.Recipient)
                  .FirstOrDefaultAsync(m => m.Id == messageId);
        }
       
       
        public Task<PaginatedResult<MessageDto>> GetMessagesForMember(MessageParams messageParams)
        {
            var query = _Context.Messages.OrderByDescending(m=>m.MessageSent).AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(m => m.RecipientId == messageParams.MemberId),
                "Outbox"=>query.Where(m=>m.SenderId == messageParams.MemberId)
            };

            var queryDto = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

          return PaginationHelper.CreateAsync(queryDto, messageParams.PageNumber, messageParams.PageSize);

        }

        public async Task<IReadOnlyList<MessageDto>> GetMessageThread(string currentMemberId, string recipientId)
        {

            await _Context.Messages
                 .Where(m => m.RecipientId == currentMemberId
                 && m.SenderId == recipientId         
                 && m.DateRead == null)
        .ExecuteUpdateAsync(setters =>
            setters.SetProperty(m => m.DateRead, DateTime.UtcNow));

            return await _Context.Messages.Where(m => (m.SenderId == currentMemberId && m.RecipientId == recipientId) ||
                  (m.SenderId == recipientId && m.RecipientId == currentMemberId))
                     .OrderBy(m => m.MessageSent).ProjectTo<MessageDto>(_mapper.ConfigurationProvider).ToListAsync();

        }

        public void RemoveMessage(Message message)
        {
            _Context.Messages.Remove(message);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _Context.SaveChangesAsync()>0;
        }
    }
}
