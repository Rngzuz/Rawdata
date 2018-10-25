namespace Rawdata.Data.Models.Relationships
{
    public class PostTag
    {
        public int PostId { get; set; }
        public Post Post { get; set; }

        public string TagName { get; set; }
        public Tag Tag { get; set; } 
    }
}