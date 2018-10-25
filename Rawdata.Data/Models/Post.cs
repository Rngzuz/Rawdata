using System;
using System.Collections.Generic;
using Rawdata.Data.Models.Relationships;

namespace Rawdata.Data.Models
{
    public class Post
    {
        //TODO Add nullable property for array of post tags 
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int Score { get; set; }
        public string Body { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string Title { get; set; }

        public int? ParentId { get; set; }
        public Post Parent { get; set; }
        public ICollection<Post> ChildrenPosts { get; set; }

        public int? AcceptedAnswerId { get; set; }
        public Post AcceptedAnswer { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<PostTag> PostTags { get; set; }

        public ICollection<PostLink> LinkedToPosts { get; set; }
        public ICollection<PostLink> LinkedByPosts { get; set; }
    }
}