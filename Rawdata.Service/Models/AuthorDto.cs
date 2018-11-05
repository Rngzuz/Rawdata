using System;

namespace Rawdata.Service.Models
{
    public class AuthorDto
    {
        public string DisplayName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Location { get; set; }
        public int? Age { get; set; }
        public AuthorDtoLink Links { get; set; }

        public class AuthorDtoLink
        {
            public string Self { get; set; }
            // public string Questions { get; set; }
            // public string Comments { get; set; }
        }
    }
}
