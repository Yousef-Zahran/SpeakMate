using SpeakMate.Core.Entities;
using SpeakMate.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Infrastructure.Security
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IConfiguration config,UserManager<AppUser>userManager)
        {
            _config = config;
            _userManager = userManager;
        }
        public async Task<string> CreateTokenAsync(AppUser user)

        {
            var key = _config["Jwt:Key"]?? throw new Exception("Can't get the key");
            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Email,user.Email!),
                new(ClaimTypes.NameIdentifier,user.Id),
                new(ClaimTypes.Name,user.DisplayName)

            };

            var roles =await _userManager.GetRolesAsync(user);
            authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: authClaims,
                signingCredentials: new SigningCredentials(authkey, SecurityAlgorithms.HmacSha256Signature),
                expires: DateTime.Now.AddDays(1)

                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
