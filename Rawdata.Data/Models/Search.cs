namespace Rawdata.Data.Models
{
    public class Search
    {
        public int? UserId { get; set; }
        public User User { get; set; }

        public string SearchText { get; set; }
        
    }
}