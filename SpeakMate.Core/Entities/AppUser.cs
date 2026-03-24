using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Entities
{
    public class AppUser : IdentityUser
    {
    
        public required string DisplayName { get; set; }
        public string? ImageUrl { get; set; }

        public string? RefreshToken { get; set; }
        public  DateTime? RefreshTokenExpiry { get; set; }
        public Member Member { get; set; } = null!;
    }
}
