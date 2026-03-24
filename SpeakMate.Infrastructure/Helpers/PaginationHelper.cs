using SpeakMate.Core.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Infrastructure.Pagination
{
    public class PaginationHelper
    {
        public static async Task<PaginatedResult<T>> CreateAsync<T>(IQueryable<T> query, int pageNumber, int pageSize)
        {

            var count = await query.CountAsync();

            var items= await query.Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();


            return new PaginatedResult<T>
            {
                MetaData = new PaginationMetaData
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = count,
                    TotalPages = (int)Math.Ceiling(count / (double)pageSize)

                },
                Items = items,
            };

        }
    }
}
