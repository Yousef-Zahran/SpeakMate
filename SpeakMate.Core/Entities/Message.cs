using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Entities
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); 
        public required string Content { get; set; }
        public DateTime MessageSent { get; set; }= DateTime.UtcNow;
        public DateTime? DateRead { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }

        public required string SenderId { get; set; }
        public Member Sender { get; set; } = null!;

        public required string RecipientId { get; set; }
        public Member Recipient { get; set; } = null!;

        public string Language { get; set; } = null!;      // auto-detected
        public bool LanguageIsCorrect { get; set; } = true; // partner can flag
        public List<MessageCorrection> Corrections { get; set; } = [];

    }
}
