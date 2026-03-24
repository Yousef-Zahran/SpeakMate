using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Entities
{
    public class MemberStats
    {
        public int Id { get; set; }

        public string MemberId { get; set; } = null!;
        public Member Member { get; set; } = null!;

        public int TotalMessagesSent { get; set; }
        public int CorrectionsReceived { get; set; }
        public int CorrectionsAccepted { get; set; }
        public int CurrentStreak { get; set; }          
    }
}
