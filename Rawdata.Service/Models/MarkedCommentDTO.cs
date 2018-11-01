using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rawdata.Data.Models;

namespace Rawdata.Service.Models
{
    public class MarkedCommentDto
    {
        [JsonProperty(PropertyName = "commentId")]
        public int CommentId { get; set; }

        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }

        public MarkedCommentDto()
        {
        }

        public MarkedCommentDto(MarkedComment favoriteComment)
        {
            CommentId = favoriteComment.CommentId;
            Note = favoriteComment.Note;
        }
    }
}
