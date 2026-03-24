using AutoMapper;
using SpeakMate.API.Extensions;
using SpeakMate.Core.DTOs;
using SpeakMate.Core.Entities;
using SpeakMate.Core.Interfaces;
using SpeakMate.Core.Pagination;
using SpeakMate.Core.Params;
using SpeakMate.Infrastructure.Helpers;
using SpeakMate.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SpeakMate.API.Controllers
{
    [Authorize]
    public class MessageController : BaseApiController
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapping;
        private readonly LanguageDetectionService _languageDetector;
        private readonly ITranslationService _translationService;

        public MessageController(
            IMemberRepository memberRepository,
            IMessageRepository messageRepository,
            IMapper mapping,
            LanguageDetectionService languageDetector,
            ITranslationService translationService
            )
        {
            _memberRepository = memberRepository;
            _messageRepository = messageRepository;
            _mapping = mapping;
            _languageDetector=languageDetector;
            _translationService=translationService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessage(CreateMessageDto createMessageDto)
        {
            var sender =await _memberRepository.GetMemberByIdAsync(User.GetMemberId());
            var recipient =await _memberRepository.GetMemberByIdAsync(createMessageDto.RecipientId);

            if (sender == null || recipient == null || sender.Id == createMessageDto.RecipientId) {

                return BadRequest("Cannot send the message");         
            }
            var message = new Message
            {
                SenderId = sender.Id,
                Content = createMessageDto.Content,
                RecipientId = recipient.Id,
                Sender= sender,
                Recipient=recipient,
                Language= _languageDetector.Detect(createMessageDto.Content)
            };

            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync()) return Ok(_mapping.Map<MessageDto>(message));
            
            return BadRequest("Failed to send message");
            

        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MessageDto>>> GetMessages([FromQuery]MessageParams messageParams ) {

            var memberId= User.GetMemberId();
            messageParams.MemberId = memberId;
            var messages = await _messageRepository.GetMessagesForMember(messageParams);
            return Ok(messages);


        }
        [HttpGet("Thread")]
        public async Task<ActionResult<IReadOnlyList<MessageDto>>> GetMessageThread(string RecipientId)
        {
            string CurrentMemberId = User.GetMemberId();    
            var messages=  await _messageRepository.GetMessageThread(CurrentMemberId,RecipientId);
            return Ok(messages);
        }

        [HttpGet("{messageId}/translate")]
        public async Task<ActionResult<string>> TranslateMessage(string messageId)
        {
            var message = await _messageRepository.GetMessage(messageId);

            if (message == null) return NotFound();

            var currentMember = await _memberRepository
                .GetMemberByIdAsync(User.GetMemberId());

            var translation = await _translationService.TranslateAsync(
                message.Content,
                message.Language,           
                currentMember.NativeLanguage 
            );

            return Ok(new { translation });
        }

    }
}
