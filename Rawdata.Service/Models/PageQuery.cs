namespace Rawdata.Service.Models
{
    public class PageQuery
    {
        // Search query that defaults to null
        public string Search { get; set; }

        private int _Page;

        public int Page
        {
            // Current page should be a positive value and greater than 0
            get { return _Page < 1 ? 1 : _Page; }
            set { _Page = value; }
        }

        // Default page size (is 0 if not specified)
        private int _Size = 5;

        public int Size
        {
            // Page size should not be less than 1 or negative
            get { return _Size < 1 ? 1 : _Size; }
            set { _Size = value; }
        }
    }
}
