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
    public class SavedWordRepository : ISavedWordRepository
    {
        private readonly AppDbContext _context;
        private readonly LanguageDetectionService _langDetector;
        private readonly ITranslationService _translationService;
        private readonly IMapper _mapper;
        private readonly IMemberRepository _memberRepository;
        private readonly IMessageRepository _messageRepository;

        public SavedWordRepository(
            AppDbContext context,
            LanguageDetectionService langDetector,
            ITranslationService translationService,
            IMapper mapper,
            IMemberRepository memberRepository,
            IMessageRepository messageRepository
            
            )
        {
            _context = context;
            _langDetector = langDetector;
            _translationService = translationService;
            _mapper = mapper;
            _memberRepository = memberRepository;
            _messageRepository = messageRepository;
        }

        public async Task<SavedWordDto?> SaveWordAsync(string memberId, CreateSavedWordDto dto)
        {
            
            var member = await _memberRepository.GetMemberByIdAsync( memberId );    
            if (member == null) return null;

            // get context sentence from message if provided
            string? context = null;
            string? detectedLang = null;
            if (dto.MessageId != null)
            {
                var message = await _messageRepository.GetMessage(dto.MessageId);
                context = message?.Content;
                detectedLang = message?.Language;
            }

            // translate to member's native language
            var translation = await _translationService.TranslateAsync(
                dto.Word,
                detectedLang!,
                member.NativeLanguage);

            var savedWord = new SavedWord
            {
                MemberId = memberId,
                Word = dto.Word,
                Language = detectedLang!,
                Translation = translation,
                Context = context,
                MessageId = dto.MessageId,
                SavedAt = DateTime.UtcNow
            };

            await _context.SavedWords.AddAsync(savedWord);
            await SaveAllAsync();

            return _mapper.Map<SavedWordDto>(savedWord);
        }

        public async Task<IEnumerable<SavedWordDto>> GetSavedWordsAsync(string memberId)
        {
            var words = await _context.SavedWords
                .Where(w => w.MemberId == memberId)
                .OrderByDescending(w => w.SavedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<SavedWordDto>>(words);
        }

        public async Task<SavedWord?> GetByIdAsync(string id)
            => await _context.SavedWords
                .FirstOrDefaultAsync(w => w.Id == id);

        public async Task<bool> DeleteWordAsync(string id, string memberId)
        {
            var word = await _context.SavedWords
                .FirstOrDefaultAsync(w => w.Id == id && w.MemberId == memberId);

            if (word == null) return false;

            _context.SavedWords.Remove(word);
            return await SaveAllAsync();
        }

        public async Task<bool> SaveAllAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}

