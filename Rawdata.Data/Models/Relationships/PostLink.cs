namespace Rawdata.Data.Models.Relationships
{
    public class PostLink
    {
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int LinkedId { get; set; }
        public Post LinkedPost { get; set; }
    }
}