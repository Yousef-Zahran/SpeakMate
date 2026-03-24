using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.DTOs
{
    public class SavedWordDto
    {
        public string Id { get; set; } = null!;
        public string Word { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string? Translation { get; set; }
        public string? Context { get; set; }    // original sentence
        public DateTime SavedAt { get; set; }
    }
}
