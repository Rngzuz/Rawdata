using System;

namespace Rawdata.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}