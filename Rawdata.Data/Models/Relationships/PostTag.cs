namespace Rawdata.Data.Models.Relationships
{
    public class PostTag
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public string TagName { get; set; }
        public Tag Tag { get; set; } 
    }
}