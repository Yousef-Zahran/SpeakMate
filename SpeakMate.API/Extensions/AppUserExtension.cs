using SpeakMate.Core.DTOs;
using SpeakMate.Core.Entities;
using SpeakMate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.API.Extensions
{
    public static class AppUserExtension
    {
        public static async Task<UserDto> ToDto(this AppUser user,ITokenService _tokenService)
        {

            return new UserDto()
            {
                Id= user.Id,
                Name = user.DisplayName,
                Email = user.Email!,
                ImageURL= user.ImageUrl,
                Token =await _tokenService.CreateTokenAsync(user),
            };

        }
    }
}
