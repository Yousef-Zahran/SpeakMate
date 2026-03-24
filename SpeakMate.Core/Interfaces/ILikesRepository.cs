using SpeakMate.Core.Entities;
using SpeakMate.Core.Pagination;
using SpeakMate.Core.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Interfaces
{
    public interface ILikesRepository
    {
        Task<MemberLike> GetMemberLike(string  sourceMemberId, string targetMemberId);

        Task<PaginatedResult<Member>>GetMemberLikes(LikeParams likeParams);

        Task<IReadOnlyList<string>> GetCurrenrMemberLikeIds(string MemberId);

        void DeleteLike(MemberLike Like);

        void AddLike(MemberLike Like);

        Task<bool> SaveAllChanges();


    }
}
