using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.DTOs
{
    public class MemberUpdateDto
    {
        public string? DisplayName { get; set; }
        public string? Discription { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

    }
}
