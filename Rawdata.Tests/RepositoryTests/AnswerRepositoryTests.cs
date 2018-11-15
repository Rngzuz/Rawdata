using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories;
using Xunit;

namespace Rawdata.Tests.RepositoryTestsFolder
{
    public class AnswerRepositoryTests
    {
        [Fact]
        public void Answers_GetAll_ReturnsNonZero()
        {
            DataContext db = new DataContext();
            AnswerRepository repo = new AnswerRepository(db);

            IEnumerable<Answer> answers = repo.GetAllAsync().Result;
            Assert.True(answers.Count() != 0);
        }

        [Fact]
        public void Answers_GetById_Success()
        {
            DataContext db = new DataContext();
            AnswerRepository repo = new AnswerRepository(db);

            Answer answer = repo.GetById(71).Result;

            Assert.Equal(71, answer.Id);
            Assert.Equal(19, answer.Parent.Id);
            Assert.Contains("<p>Here's a general description of a technique", answer.Body);
            Assert.Equal(49, answer.Author.Id);
            
        }
    }
}