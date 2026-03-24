using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpeakMate.Core.DTOs.Translation
{
    public class GroqResponseDto
    {
        [JsonPropertyName("choices")]
        public List<GroqChoice> Choices { get; set; }
    }
}
