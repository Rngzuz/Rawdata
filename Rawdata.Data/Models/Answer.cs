namespace Rawdata.Data.Models
{
    public class Answer : Post
    {
        public int ParentId { get; set; }
        public Question Parent { get; set; }

    }
}