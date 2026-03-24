using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpeakMate.Core.Entities
{
    public class Member
    {
        public string Id { get; set; } = null!;
        public required string DisplayName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? ImageUrl { get; set; }
        public required string Gender { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string? Discription { get; set; }
        public required string NativeLanguage { get; set; }
        public required string ForeignLanguageLearning { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public List<Photo> Photos { get; set; } = [];

        public List<MemberLike> LikedByMembers { get; set; } = [];
        public List<MemberLike> LikedMembers { get; set; } = [];

        [JsonIgnore]
        public List<Message> MessagesSent { get; set; } = [];
        [JsonIgnore]
        public List<Message> MessagesReceived { get; set; } = [];

        [ForeignKey(nameof(Id))]
        public AppUser User { get; set; } = null!;

        public string? ForeignLanguageLevel { get; set; }  
        public string? LearningGoal { get; set; }         
        public bool IsAvailableToChat { get; set; } = true;

        public List<SavedWord> SavedWords { get; set; } = [];
        public MemberStats? Stats { get; set; }

    }
}
