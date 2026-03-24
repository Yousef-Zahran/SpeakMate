using SpeakMate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.DTOs
{
    public class UserSeedDto
    {
        public required string Id { get; set; } 
        public required string DisplayName { get; set; }
        public required string Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? ImageUrl { get; set; }
        public required string Gender { get; set; }
        public DateTime Created { get; set; } 
        public DateTime LastActive { get; set; }
        public string? Description { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required string NativeLanguage { get; set; }
        public required string ForeignLanguageLearning { get; set; }

    }
}
