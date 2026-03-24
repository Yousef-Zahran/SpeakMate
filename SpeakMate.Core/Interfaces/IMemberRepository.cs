using SpeakMate.Core.Entities;
using SpeakMate.Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Interfaces
{
    public interface IMemberRepository
    {
        void Update(Member member);
        Task<bool> SaveAllAsync();
        Task<PaginatedResult<Member>> GetAllAsync(MemberParams memberParams);
        Task<Member?>GetMemberByIdAsync(string id);
        Task<IReadOnlyList<Photo>>GetAllPhotosForMemberAsync(string id);
        Task<Member?>GetMemberForUpdate(string id);
    }
}
