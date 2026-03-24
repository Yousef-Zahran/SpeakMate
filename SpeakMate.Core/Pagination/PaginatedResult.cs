using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Pagination
{
    public class PaginatedResult<T>
    {
        public PaginationMetaData MetaData { get; set; } = default!;

        public List<T> Items { get; set; } = [];

    }
}
