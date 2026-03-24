using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Entities
{
    public class MemberLike
    {
        public required string SourceMemberId { get; set; }
        public Member SourceMember { get; set; } = null!;

        public required string TargetMemberId { get; set; }
        public Member TargetMember { get; set; }=null!;

    }
}
