using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SpeakMate.API.Extensions;
using SpeakMate.Core.DTOs;
using SpeakMate.Core.Entities;
using SpeakMate.Core.Interfaces;
using SpeakMate.Core.Repositories;
using SpeakMate.Infrastructure.Services;
using System.Security.Claims;

namespace SpeakMate.API.Hubs
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IUserTracker _tracker;
        private readonly IMapper _mapping;
        private readonly IMessageCorrectionRepository _correctionRepo;
        private readonly LanguageDetectionService _languageDetector;
        public MessageHub(IMessageRepository repo, IUserTracker tracker, IMapper mapping, IMessageCorrectionRepository correctionRepo, LanguageDetectionService languageDetector)
        {
            _messageRepo = repo;
            _tracker = tracker;
            _mapping = mapping;
            _correctionRepo = correctionRepo;
            _languageDetector = languageDetector;
        }

        

        public override async Task OnConnectedAsync()
        {
            var currentUserId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var recipientId = Context.GetHttpContext()?.Request.Query["recipientId"].ToString();

            if (!string.IsNullOrEmpty(currentUserId))
            {
                
                await _tracker.AddAsync(Context.ConnectionId, currentUserId);

                if (!string.IsNullOrEmpty(recipientId))
                {
                    var thread = await _messageRepo.GetMessageThread(currentUserId, recipientId);
                    await Clients.Caller.SendAsync("ReceiveMessageThread", thread);

                    bool hadUnread = thread.Any(m => m.SenderId == recipientId && m.DateRead == null);
                    if (hadUnread)
                        await Clients.User(recipientId).SendAsync("MessagesRead", currentUserId);
                }
            }
            Console.WriteLine($"Connected: {currentUserId} | ConnId: {Context.ConnectionId} | recp{recipientId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            await _tracker.RemoveAsync(Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }


        public async Task SendMessage(CreateMessageDto dto)
        {
            try
            {
                var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (senderId == null)
                    throw new HubException("Not authenticated.");

                if (senderId == dto.RecipientId)
                    throw new HubException("You cannot message yourself.");

                var message = new Message
                {
                    SenderId = senderId,
                    RecipientId = dto.RecipientId,
                    Content = dto.Content,
                    MessageSent = DateTime.UtcNow,
                    Language = _languageDetector.Detect(dto.Content)?? "unknown"
                };

                _messageRepo.AddMessage(message);

                if (!await _messageRepo.SaveAllAsync())
                    throw new HubException("Failed to save message.");

                var fullMessage = await _messageRepo.GetMessage(message.Id);

                if (fullMessage == null)
                    throw new HubException("Message not found after saving.");

                var messageDto = _mapping.Map<MessageDto>(fullMessage);

               
                Console.WriteLine($"Sender: {senderId}");
                Console.WriteLine($"Recipient: {dto.RecipientId}");
                await Clients.User(dto.RecipientId).SendAsync("NewMessage", messageDto);

                await Clients.Caller.SendAsync("NewMessage", messageDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SendMessage ERROR: {ex}");
                throw new HubException($"Internal error: {ex.Message}");
            }
        }

        public async Task DeleteMessage(string messageId)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var message = await _messageRepo.GetMessage(messageId);

            if (message == null)
                throw new HubException("Message not found.");

            if (message.SenderId != userId && message.RecipientId != userId)
                throw new HubException("Unauthorized.");

            _messageRepo.RemoveMessage(message);

            if (!await _messageRepo.SaveAllAsync())
                throw new HubException("Failed to delete message.");

            await Clients.User(message.SenderId).SendAsync("MessageDeleted", messageId);
            await Clients.User(message.RecipientId).SendAsync("MessageDeleted", messageId);
        }

        public async Task MarkAsRead(string senderId)
        {
            var currentUserId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserId == null)
                throw new HubException("Not authenticated.");

            var thread = await _messageRepo.GetMessageThread(currentUserId, senderId);

            await Clients.User(senderId).SendAsync("MessagesRead", currentUserId);
        }

        public async Task SendTyping(string recipientId)
        {
            var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
            await Clients.User(recipientId).SendAsync("UserTyping", username);
        }

        public async Task SendCorrection(CreateMessageCorrectionDto dto)
        {
            var correctorId = Context.User!.GetMemberId();

            var correction = await _correctionRepo.AddCorrectionAsync(correctorId, dto);
            if (correction == null) return;

            var message = await _messageRepo.GetMessage(dto.MessageId);
            if (message == null) return;

            await Clients.User(message.SenderId)
                .SendAsync("ReceiveCorrection", correction);

            await Clients.User(correctorId)
                .SendAsync("ReceiveCorrection", correction);
        }

        public async Task AcceptCorrection(string correctionId)
        {
            var userId = Context.User!.GetMemberId();
            var success = await _correctionRepo.AcceptCorrectionAsync(correctionId, userId);

            if (!success) return;

            var correction = await _correctionRepo.GetByIdAsync(correctionId);
            var message = await _messageRepo.GetMessage(correction!.MessageId);

            await Clients.User(message!.SenderId)
                .SendAsync("CorrectionAccepted", correctionId);

            await Clients.User(message.RecipientId)
                .SendAsync("CorrectionAccepted", correctionId);
        }
    }
}