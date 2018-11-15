using System.Collections.Generic;
using Rawdata.Data.Models.Relationships;

namespace Rawdata.Data.Models
{
    public class Tag
    {
        public string Name { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
    }
}