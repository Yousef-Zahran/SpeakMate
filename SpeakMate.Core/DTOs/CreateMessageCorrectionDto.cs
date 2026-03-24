using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.DTOs
{
    public class CreateMessageCorrectionDto
    {
        public string MessageId { get; set; }
        public string CorrectedText { get; set; } = null!;
        public string? Explanation { get; set; }
    }
}
