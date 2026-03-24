using SpeakMate.API.Extensions;
using SpeakMate.Core.DTOs;
using SpeakMate.Core.Entities;
using SpeakMate.Core.Interfaces;
using SpeakMate.Infrastructure.Data;
using SpeakMate.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace SpeakMate.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;

        public ITokenService _tokenService { get; }

        public AccountController(UserManager<AppUser> userManager,ITokenService tokenService) {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> SignUp(RegisterDto registerDto)
        {
          
            var user = new AppUser { 
                UserName= registerDto.DisplayName,
                DisplayName = registerDto.DisplayName,
                Email= registerDto.Email,
                Member= new Member
                {
                    City = registerDto.City,
                    Country = registerDto.Country,
                    DisplayName = registerDto.DisplayName,
                    Gender  = registerDto.Gender,
                    DateOfBirth = registerDto.DateOfBirth,
                   NativeLanguage= registerDto.NativeLanguage,
                   ForeignLanguageLearning= registerDto.ForeignLanguageLearning

                }
            };

          var result= await _userManager.CreateAsync(user,registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                    ModelState.AddModelError("Identity", error.Description);

                }
                return ValidationProblem();
            }
            return await user.ToDto(_tokenService);

        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> LogIn(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized("The email is not correct!");
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) return Unauthorized("Invalid password");
            return await user.ToDto(_tokenService);
            
        }


       

   }

}
