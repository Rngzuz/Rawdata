using System;
using System.Collections.Generic;

namespace Rawdata.Service.Models
{
    public class QuestionDto
    {
        public int Score { get; set; }
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }
        public string Title { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string AuthorDisplayName { get; set; }
        public IList<AnswerDto> Answers { get; set; }
        public IList<CommentDto> Comments { get; set; }
        public QuestionDtoLink Links { get; set; }

        public class QuestionDtoLink
        {
            public string Self { get; set; }
            public string Author { get; set; }
        }
    }
}
