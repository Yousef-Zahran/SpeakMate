using SpeakMate.Core.Entities;
using SpeakMate.Core.Interfaces;
using SpeakMate.Core.Pagination;
using SpeakMate.Core.Params;
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
    public class LikesRepository(AppDbContext _dbContext ) : ILikesRepository
    {
        public void AddLike(MemberLike Like)
        {
             _dbContext.Likes.Add(Like);
        }

        public void DeleteLike(MemberLike Like)
        {
           _dbContext.Likes.Remove(Like);
        }

        public async Task<IReadOnlyList<string>> GetCurrenrMemberLikeIds(string MemberId)
        {
         return await _dbContext.Likes
                .Where(l=>l.SourceMemberId==MemberId)
                .Select(l=>l.TargetMemberId)
                .ToListAsync(); 
        }

        public async Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId)
        {
            return await _dbContext.Likes.FindAsync(sourceMemberId,targetMemberId);
        }

        public async Task<PaginatedResult<Member>> GetMemberLikes(LikeParams likeParams)
        {
           //return await _dbContext.Likes.FindAsync();
           var query = _dbContext.Likes.AsQueryable();
            IQueryable<Member> result;
            switch (likeParams.Predicate)
            {
                case "likedBy":
                    result = query
                        .Where(l => l.TargetMemberId == likeParams.MemberId)
                        .Select(m => m.TargetMember);
                    break;

                case "liked":
                    result = query
                        .Where(l => l.SourceMemberId == likeParams.MemberId)
                        .Select(m => m.TargetMember);
                    break;
                default: //mutual
                    var likeIds = await GetCurrenrMemberLikeIds(likeParams.MemberId);

                    result = query
                        .Where(l => l.TargetMemberId == likeParams.MemberId
                        && likeIds.Contains(l.SourceMemberId))
                        .Select(m => m.SourceMember);
                    break;

            }

            return await PaginationHelper.CreateAsync(result, likeParams.PageNumber, likeParams.PageSize);

        }

        public async Task<bool> SaveAllChanges()
        {
            return await _dbContext.SaveChangesAsync()>0;
        }
    }
}
