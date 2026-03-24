using SpeakMate.Core.Entities;
using SpeakMate.Core.Interfaces;
using SpeakMate.Core.Pagination;
using SpeakMate.Infrastructure.Data;
using SpeakMate.Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Infrastructure.Repositories
{
    public class MemberRepository(AppDbContext _Context) : IMemberRepository
    {
        public async Task<PaginatedResult<Member>> GetAllAsync(MemberParams memberParams)
        {
            
            var query = _Context.Members.AsQueryable();

            query = query.Where(m => m.Id != memberParams.CurrentMemberId);
            if (memberParams.Gender!=null)
            {
                query = query.Where(m => m.Gender == memberParams.Gender);
            }

            if ((memberParams.NativeLanguage != null) && (memberParams.ForeignLanguageLearning != null))
            {
             query= query.Where(
                 m=>m.NativeLanguage == memberParams.ForeignLanguageLearning
             &&
                 m.ForeignLanguageLearning==memberParams.NativeLanguage
                               );
            }
            

            query = memberParams.OrderBy switch
            {
                "created" => query.OrderByDescending(m => m.Created),
                _=> query.OrderByDescending(m=>m.LastActive)
            };

     
            return await PaginationHelper.CreateAsync(query,
                pageNumber: memberParams.PageNumber, pageSize: memberParams.PageSize);
        }
        public async Task<IReadOnlyList<Photo>> GetAllPhotosForMemberAsync(string id)
        {
            return await _Context.Members
                 .Where(m => m.Id == id)
                 .SelectMany(x => x.Photos)
                .ToListAsync(); 

        }

        public async Task<Member?> GetMemberByIdAsync(string id)
        {
          return await _Context.Members.FirstAsync(m => m.Id == id);  
        }

        public Task<Member?> GetMemberForUpdate(string id)
        {
            return _Context.Members
                .Include(m => m.User)
                .Include(m=>m.Photos)
                .SingleOrDefaultAsync(m=>m.Id==id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _Context.SaveChangesAsync()>0;
        }

        public void Update(Member member)
        {
           _Context.Entry(member).State = EntityState.Modified;
        }
    }
}
