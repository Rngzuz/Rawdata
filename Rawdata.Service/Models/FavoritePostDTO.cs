using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rawdata.Service.Models
{
    public class FavoritePostDTO
    {
        [JsonProperty(PropertyName = "postId")]
        public int PostId { get; set; }

        [JsonProperty(PropertyName = "note")]
        public int Note { get; set; }
    }
}
