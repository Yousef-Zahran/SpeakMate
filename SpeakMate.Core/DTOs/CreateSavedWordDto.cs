using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.DTOs
{
    public class CreateSavedWordDto
    {
        public string Word { get; set; } = null!;
        public string? MessageId { get; set; }  
    }
}
