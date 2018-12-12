using System.Collections.Generic;

namespace Rawdata.Data.Models
{
    public class SearchResult
    {
        public int PostId { get; set; }
        public Post Post { get; set; }
        public double? Rank { get; set; }
        public List<string> Excerpts { get; set;}
    }
}