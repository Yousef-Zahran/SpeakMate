using SpeakMate.Core.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Pagination
{
    public class MemberParams: PagingParams
    {
        public string? CurrentMemberId { get; set; }
        public string? Gender { get; set; }
        public string OrderBy { get; set; } = "lastActive";
        public string? NativeLanguage { get; set; }
        public string? ForeignLanguageLearning { get; set; }

    }
}
