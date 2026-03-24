using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.DTOs
{
    public class CreateMessageDto
    {
        public required string Content { get; set; }
        public required string RecipientId { get; set; }

    }
}
