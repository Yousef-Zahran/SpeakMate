using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Entities
{
    public class MessageCorrection
    {
        public string Id { get; set; }= Guid.NewGuid().ToString();

        // The original wrong message
        public required string MessageId { get; set; }
        public Message Message { get; set; } = null!;

        // Who corrected it
        public required string CorrectedById { get; set; }
        public Member CorrectedBy { get; set; } = null!;

        public string CorrectedText { get; set; } = null!;
        public string? Explanation { get; set; } // "should use past tense here"

        public bool IsAccepted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? WrongPartStart { get; set; }
        public int? WrongPartEnd { get; set; }
        public int? CorrectPartStart { get; set; }
        public int? CorrectPartEnd { get; set; }
    }
}
