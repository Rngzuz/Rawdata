using System;
using System.Collections.Generic;

namespace Rawdata.Data.Models
{
    public class PaginatedResult<T> where T : class
    {
        public IList<T> Items { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
    }
}
