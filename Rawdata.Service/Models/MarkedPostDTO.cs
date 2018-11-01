using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rawdata.Data.Models;

namespace Rawdata.Service.Models
{
    public class MarkedPostDto
    {
        [JsonProperty(PropertyName = "postId")]
        public int PostId { get; set; }

        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }

        public MarkedPostDto()
        {
        }

        public MarkedPostDto(MarkedPost markedPost)
        {
            PostId = markedPost.PostId;
            Note = markedPost.Note;
        }
    }
}
