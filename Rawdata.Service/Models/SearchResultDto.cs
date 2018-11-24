using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rawdata.Service.Models
{
    public class SearchResultDto
    {
        public string Body { get; set; }
        public int Score { get; set; }
        public float Rank { get; set; }
        public DateTime CreationDate { get; set; }
        public string AuthorDisplayName { get; set; }
        public bool Marked { get; set; }
        public string Note { get; set; }
    }
}
