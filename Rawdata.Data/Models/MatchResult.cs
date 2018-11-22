namespace Rawdata.Data.Models
{
    public class MatchResult
    {
        public int PostId { get; set; }
        public Post Post { get; set; }

        public float? Rank { get; set; }
    }
}