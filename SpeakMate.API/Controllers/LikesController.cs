using SpeakMate.API.Extensions;
using SpeakMate.Core.Entities;
using SpeakMate.Core.Interfaces;
using SpeakMate.Core.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpeakMate.API.Controllers
{
    [Authorize]
    public class LikesController(ILikesRepository _likesRepository) : BaseApiController
    {
        [HttpPost("{targetMemberId}")]
      
        public async Task<ActionResult> ToggleLike(string targetMemberId)
        {
            var memberId= User.GetMemberId();
            if (memberId==targetMemberId) return BadRequest("Cannot like yourself");
            
            var like = await _likesRepository.GetMemberLike(memberId, targetMemberId);
            if (like == null)
            {
                var newLike = new MemberLike
                {
                    SourceMemberId = memberId,
                    TargetMemberId = targetMemberId
                };

                _likesRepository.AddLike(newLike);
            }
            else
            {
                _likesRepository.DeleteLike(like);
            }
            if (await _likesRepository.SaveAllChanges()) return Ok();
            return BadRequest("Failed to update like");
        }

        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetCurrenrMemberLikeIds()
        {
            return Ok(await _likesRepository.GetCurrenrMemberLikeIds(User.GetMemberId()));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>>GetMemberLikes([FromQuery]LikeParams likeParams)
        {
         likeParams.MemberId = User.GetMemberId();
         var members=   await _likesRepository.GetMemberLikes(likeParams);
            return Ok(members);
        }
    }
}
