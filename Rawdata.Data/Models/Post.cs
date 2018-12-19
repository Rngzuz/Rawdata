using System;
using System.Collections.Generic;
using Rawdata.Data.Models.Relationships;

namespace Rawdata.Data.Models
{
    public abstract class Post
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int TypeId { get; set; }
        public string Body { get; set; }
        public DateTime? CreationDate { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<PostLink> LinkedToPosts { get; set; }
        public ICollection<PostLink> LinkedByPosts { get; set; }

        public ICollection<MarkedPost> MarkedPosts { get; set; }
    }
}