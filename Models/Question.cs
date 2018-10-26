using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("posts_question")]
    public class Question : Post
    {
        [Column("accepted_answer_id")]
        public int AcceptedAnswerId { get; set; }

        [Column("closed_date")]
        public DateTime ClosedDate { get; set; }

        [Column("title")]
        public string Title { get; set; }
    }
}
