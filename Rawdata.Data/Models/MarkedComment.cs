namespace Rawdata.Data.Models
{
    public class MarkedComment
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }

        public string Note { get; set; }
    }
}