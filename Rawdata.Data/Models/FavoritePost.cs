namespace Rawdata.Data.Models
{
    public class FavoritePost
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public string Note { get; set; }
    }
}