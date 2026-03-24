using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Params
{
    public class MessageParams : PagingParams
    {
        public string? MemberId { get; set; }
        public string Container { get; set; } = "Inbox";
    }
}
