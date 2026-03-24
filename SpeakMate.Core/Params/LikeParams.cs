using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Params
{
    public class LikeParams :PagingParams
    {
        public string Predicate { get; set; } = "liked";
        public string MemberId { get; set; } = "";
    }
}
