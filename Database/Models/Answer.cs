using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("posts_answer")]
    public class Answer : Post
    {
        [Column("parent_id")]
        public int ParentId { get; set; }
    }
}
