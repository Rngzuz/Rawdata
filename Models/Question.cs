using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class Question : Post
    {
        [Column("accepted_answer_id")]
        public int? AcceptedAnswerId { get; set; }
        public Answer AcceptedAnswer { get; set; }

        [Column("closed_date")]
        public DateTime? ClosedDate { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("link_id")]
        public int? LinkId { get; set; }
        public Question Link { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
