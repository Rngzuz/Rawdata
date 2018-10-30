using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rawdata.Service.Models
{
    public class FavoriteCommentDTO
    {
        [JsonProperty(PropertyName = "commentId")]
        public int CommentId { get; set; }

        [JsonProperty(PropertyName = "note")]
        public int Note { get; set; }
    }
}
