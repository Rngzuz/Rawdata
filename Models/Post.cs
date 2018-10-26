using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public abstract class Post
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Column("type_id")]
        public int TypeId { get; set; }

        [Column("creation_date")]
        public DateTime CreationDate { get; set; }

        [Column("score")]
        public int Score { get; set; }

        [Column("body")]
        public string Body { get; set; }

        [Column("author_id")]
        public int AuthorId { get; set; }
    }
}
