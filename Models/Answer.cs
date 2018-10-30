using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class Answer : Post
    {
        [Column("parent_id")]
        public int ParentId { get; set; }
        public Question Parent { get; set; }
    }
}
