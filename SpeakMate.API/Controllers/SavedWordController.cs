using SpeakMate.API.Extensions;
using SpeakMate.Core.DTOs;
using SpeakMate.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SpeakMate.API.Controllers
{
    
    public class SavedWordController : BaseApiController
    {
        private readonly ISavedWordRepository _repo;

        public SavedWordController(ISavedWordRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SavedWordDto>>> GetSavedWords()
        {
            var words = await _repo.GetSavedWordsAsync(User.GetMemberId());
            return Ok(words);
        }

        [HttpPost]
        public async Task<ActionResult<SavedWordDto>> SaveWord(CreateSavedWordDto dto)
        {
            var result = await _repo.SaveWordAsync(User.GetMemberId(), dto);
            if (result == null) return NotFound("Member not found");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWord(string id)
        {
            var success = await _repo.DeleteWordAsync(id, User.GetMemberId());
            if (!success) return NotFound("Word not found");
            return NoContent();
        }
    }
}
