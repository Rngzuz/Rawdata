namespace Rawdata.Data.Models
{
    public class DeactivatedFavoriteComment :FavoriteComment
    {
        public int UserId { get; set; }
        public DeactivatedUser DeactivatedUser { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }

        public string Note { get; set; }

    }
}