using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Services;
using Xunit;

namespace Rawdata.UnitTests.Tests
{
    public class QuestionsTest
    {
        [Fact]
        public void Test_GetUserById()
        {
            DataContext db = new DataContext();
            QuestionService repo = new QuestionService(db);

            Task<Question> question = repo.GetQuestionById(19);

            Assert.Equal(164, question.Result.Score);
            Assert.Equal("What is the fastest way to get the value of π?", question.Result.Title);
        }

        [Fact]
        public void Test_QueryQuestions()
        {
            DataContext db = new DataContext();
            QuestionService repo = new QuestionService(db);

            var tags = new string[] { "mysql","sql" };

            IQueryable<Question> question = repo.QueryQuestions(null, "null", tags, false, 1, 10);

            Question q = question.FirstOrDefault();
            Assert.NotNull(q);
            Assert.Equal(1848, q.Score);
            Assert.Equal(38549, q.Id);
            Assert.Equal("Difference between INNER and OUTER joins", q.Title);
        }

        [Fact]
        public void Test_QueryQuestions2()
        {
            DataContext db = new DataContext();
            QuestionService repo = new QuestionService(db);

            
            var tags = new string[ ]{ "arrays" };
            IQueryable<Question> question = repo.QueryQuestions(1, "ALTER", tags, false, 1, 1);

            Question q = question.FirstOrDefault();
            Assert.NotNull(q);
            Assert.Equal(34, q.Score);
            Assert.Equal(5752906, q.Id);
            Assert.Equal("Is reading the `length` property of an array really that expensive an operation in JavaScript?",q.Title);
        }

    }
}
