using SpeakMate.API.Extensions;
using SpeakMate.Core.DTOs;
using SpeakMate.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SpeakMate.API.Controllers
{
   
    public class MessageCorrectionController : BaseApiController
    {
        private readonly IMessageCorrectionRepository _repo;

        public MessageCorrectionController(IMessageCorrectionRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<ActionResult<MessageCorrectionDto>> AddCorrection(
            CreateMessageCorrectionDto dto)
        {
            var result = await _repo.AddCorrectionAsync(User.GetMemberId(), dto);
            if (result == null) return NotFound("Message not found");
            return Ok(result);
        }

        [HttpPut("{id}/accept")]
        public async Task<ActionResult> AcceptCorrection(string id)
        {
            var success = await _repo.AcceptCorrectionAsync(id, User.GetMemberId());
            if (!success) return BadRequest("Could not accept correction");
            return NoContent();
        }
    }
}
