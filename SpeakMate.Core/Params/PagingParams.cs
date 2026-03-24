using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Params
{
    public class PagingParams
    {
		private const int MaxPageSize = 20;
		public int PageNumber { get; set; } = 1;

		private int pageSize=5;

		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = (value> MaxPageSize)?MaxPageSize:value; }
		}


	}
}
