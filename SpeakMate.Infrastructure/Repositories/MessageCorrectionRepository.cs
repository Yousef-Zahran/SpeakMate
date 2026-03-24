using AutoMapper;
using SpeakMate.Core.DTOs;
using SpeakMate.Core.Entities;
using SpeakMate.Core.Interfaces;
using SpeakMate.Infrastructure.Data;
using SpeakMate.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Infrastructure.Repositories
{
    public class MessageCorrectionRepository : IMessageCorrectionRepository
    {
        private readonly AppDbContext _context;
        private readonly IMessageRepository _messageRepo;
        private readonly DiffService _diff;
        private readonly IMapper _mapper;

        public MessageCorrectionRepository(
            AppDbContext context,
            IMessageRepository messageRepo,
            DiffService diff,
            IMapper mapper)
        {
            _context = context;
            _messageRepo = messageRepo;
            _diff = diff;
            _mapper = mapper;
        }

        public async Task<MessageCorrectionDto?> AddCorrectionAsync(
            string correctorUserId,
            CreateMessageCorrectionDto dto)
        {
            var message = await _messageRepo.GetMessage(dto.MessageId);
            if (message == null) return null;

            var (wrongStart, wrongEnd) = _diff.FindWrongPart(message.Content, dto.CorrectedText);
            var (correctStart, correctEnd) = _diff.FindCorrectPart(message.Content, dto.CorrectedText);

            var correction = new MessageCorrection
            {
                MessageId = dto.MessageId,
                CorrectedById = correctorUserId,
                CorrectedText = dto.CorrectedText,
                Explanation = dto.Explanation,
                WrongPartStart = wrongStart,
                WrongPartEnd = wrongEnd,
                CorrectPartStart = correctStart,
                CorrectPartEnd = correctEnd
            };

            await _context.MessageCorrections.AddAsync(correction);
            await SaveAllAsync();

            
            await _context.Entry(correction)
                .Reference(c => c.CorrectedBy)
                .LoadAsync();

            return _mapper.Map<MessageCorrectionDto>(correction);
        }

        public async Task<bool> AcceptCorrectionAsync(string correctionId, string userId)
        {
            var correction = await _context.MessageCorrections
                .Include(c => c.Message)
                .FirstOrDefaultAsync(c => c.Id == correctionId);

            if (correction == null) return false;

            
            if (correction.Message.SenderId != userId) return false;

            correction.IsAccepted = true;
            return await SaveAllAsync();
        }

        public async Task<MessageCorrection?> GetByIdAsync(string id)
            => await _context.MessageCorrections
                .Include(c => c.Message)
                .Include(c => c.CorrectedBy)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<IEnumerable<MessageCorrection>> GetCorrectionsForMessageAsync(string messageId)
            => await _context.MessageCorrections
                .Include(c => c.CorrectedBy)
                .Where(c => c.MessageId == messageId)
                .ToListAsync();


        public async Task<bool> SaveAllAsync()
            => await _context.SaveChangesAsync() > 0;
    }

}

