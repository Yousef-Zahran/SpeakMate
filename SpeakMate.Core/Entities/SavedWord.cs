using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Entities
{
    public class SavedWord
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string MemberId { get; set; } = null!;
        public Member Member { get; set; } = null!;

        public string Word { get; set; } = null!;
        public string Language { get; set; } = null!;  
        public string? Context { get; set; }             // the sentence it came from
        public string? Translation { get; set; }

        //link back to the message it came from
        public string? MessageId { get; set; }
        public Message? Message { get; set; }

        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    }
}
