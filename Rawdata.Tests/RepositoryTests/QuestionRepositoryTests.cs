using System.Collections.Generic;
using System.Linq;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories;
using Xunit;

namespace Rawdata.Tests.RepositoryTestsFolder
{
    public class QuestionRepositoryTests
    {
        [Fact]
        public void Answers_GetAll_ReturnsNonZero()
        {
            DataContext db = new DataContext();
            QuestionRepository repo = new QuestionRepository(db);

            IEnumerable<Question> questions = repo.GetAllAsync().Result;
            Assert.True(questions.Count() != 0);
        }

        [Fact]
        public void Answers_GetById_Success()
        {
            DataContext db = new DataContext();
            QuestionRepository repo = new QuestionRepository(db);

            Question question = repo.GetById(19).Result;

            Assert.Equal(19, question.Id);
            Assert.Contains("<p>Solutions are welcome in any language.", question.Body);
            Assert.Equal("What is the fastest way to get the value of π?", question.Title);
            Assert.Equal(13, question.Author.Id);
        }
    }
}