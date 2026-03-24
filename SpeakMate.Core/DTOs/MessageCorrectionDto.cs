using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.DTOs
{
    public class MessageCorrectionDto
    {
        public string Id { get; set; }
        public string MessageId { get; set; }
        public string OriginalContent { get; set; } = null!; // from Message
        public string CorrectedText { get; set; } = null!;
        public string? Explanation { get; set; }

        public int? WrongPartStart { get; set; }
        public int? WrongPartEnd { get; set; }
        public int? CorrectPartStart { get; set; }
        public int? CorrectPartEnd { get; set; }

        public string CorrectedByName { get; set; } = null!;
        public bool IsAccepted { get; set; }
    }
}
