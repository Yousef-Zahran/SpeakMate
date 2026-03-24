using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpeakMate.Core.DTOs.Translation
{
    public class GroqChoice
    {
        [JsonPropertyName("message")]
        public GroqMessage Message { get; set; }
    }
}
