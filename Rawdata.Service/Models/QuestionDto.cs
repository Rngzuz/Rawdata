using System;
using System.Collections.Generic;
using Rawdata.Data.Models;

namespace Rawdata.Service.Models
{
    public class QuestionListDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int Score { get; set; }
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }
        public string Title { get; set; }
        public DateTime? ClosedDate { get; set; }
        public IList<string> Tags { get; set; }
        public string AuthorDisplayName { get; set; }
        public QuestionDtoLink Links { get; set; }

        public class QuestionDtoLink
        {
            public string Self { get; set; }
            public string Author { get; set; }
        }
    }

    public class QuestionDto : QuestionListDto
    {
        public IList<AnswerDto> Answers { get; set; }
        public IList<CommentDto> Comments { get; set; }
    }

    public class MarkedQuestionDto : QuestionListDto
    {
        public string Note { get; set; }
    }
}
