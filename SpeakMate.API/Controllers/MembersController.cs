using SpeakMate.API.Extensions;
using SpeakMate.Core.Entities;
using SpeakMate.Core.Interfaces;
using SpeakMate.Core.Pagination;
using SpeakMate.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SpeakMate.Infrastructure.Services;

namespace SpeakMate.API.Controllers
{
    [Authorize]
    public class MembersController : BaseApiController
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IPhotoService _photoService;

        public MembersController(IMemberRepository memberRepository ,IPhotoService photoService)
        {
           _memberRepository = memberRepository;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers([FromQuery] MemberParams memberParams) {
            var memberId = User.GetMemberId();
            var member=await _memberRepository.GetMemberByIdAsync(memberId);
            memberParams.CurrentMemberId = memberId;
            memberParams.NativeLanguage = member?.NativeLanguage;
            memberParams.ForeignLanguageLearning = member?.ForeignLanguageLearning;
            var members =await _memberRepository.GetAllAsync(memberParams);

            return Ok(members);
        }
        
        [HttpGet("Id")]
        public async Task<ActionResult<AppUser>> GetMember(string id) {
         
            var member =await _memberRepository.GetMemberByIdAsync(id);
            if (member==null) return NotFound();
            return Ok(member);

        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotosById(string id)
        {

            return Ok(await _memberRepository.GetAllPhotosForMemberAsync(id));
        }

        
        [HttpPut]

        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var memberId = User.GetMemberId();
            var member =await _memberRepository.GetMemberForUpdate(memberId);
            if (member==null) return NotFound();
           
            member.City= memberUpdateDto.City??member.City;
            member.DisplayName= memberUpdateDto.DisplayName??member.DisplayName;
            member.Country= memberUpdateDto.Country??member.Country;
            member.Discription = memberUpdateDto.Discription ?? member.Discription;

            member.User.DisplayName = memberUpdateDto.DisplayName ?? member.DisplayName;
            _memberRepository.Update(member);

            if (await _memberRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update member");
        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<Photo>> AddPhoto([FromForm] IFormFile file)
        {
            var member= await _memberRepository.GetMemberForUpdate(User.GetMemberId());
            if (member==null) return BadRequest("Cannot update member");
            var result= await _photoService.UploadPhotoAsync(file);
            if (result.Error!=null) return BadRequest(result.Error.Message);
            var photo = new Photo
            { 
                Url=result.SecureUrl.AbsoluteUri,
                MemberId=member.Id,
                PublicId = result.PublicId,
            };

            if (member.Photos.Count==0) { 
                member.ImageUrl = photo.Url;  
                member.User.ImageUrl = photo.Url;
            }

            member.Photos.Add(photo);
            if(await _memberRepository.SaveAllAsync() ) return photo;
            return BadRequest("Problem adding photo");

        }
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId) {
            var member = await _memberRepository.GetMemberForUpdate(User.GetMemberId());
            if (member == null) return BadRequest("Cannot update member");
            var photo= member.Photos.SingleOrDefault(p => p.Id==photoId);
            if (photo == null||photo.Url==member.ImageUrl){
                return BadRequest("Cannot set this as a main photo");
            }
            member.ImageUrl=photo.Url;
            member.User.ImageUrl=photo.Url;
            if (await _memberRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem settin main photo");
        
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var member = await _memberRepository.GetMemberForUpdate(User.GetMemberId());
            if (member == null) return BadRequest("Cannot update member");
            var photo = member.Photos.SingleOrDefault(p => p.Id == photoId);
            if (photo == null || photo.Url == member.ImageUrl)
            {
                return BadRequest("Cannot set this as a main photo");
            }
            if (photo.PublicId != null) {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);

            }
            member.Photos.Remove(photo);
            if (await _memberRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting the photo");

        }
    }
}
