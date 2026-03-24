using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpeakMate.Core.DTOs.Translation
{
    public class GroqMessage
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
