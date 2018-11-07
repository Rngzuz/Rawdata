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

            String[] tags = new string[0];

            IQueryable<Question> question = repo.QueryQuestions(null, "null", tags, true, 2, 2);

            Question q = question.First();
            Assert.NotNull(q);
            Assert.Equal(335, q.Score);
            Assert.Equal(145154, q.Id);
            Assert.Equal("What should my Objective-C singleton look like?",q.Title);
        }

        [Fact]
        public void Test_QueryQuestions2()
        {
            DataContext db = new DataContext();
            QuestionService repo = new QuestionService(db);

            String[] tags = new string[0];

            IQueryable<Question> question = repo.QueryQuestions(1, "ALTER", tags, false, 2, 2);

            Question q = question.First();
            Assert.NotNull(q);
            Assert.Equal(34, q.Score);
            Assert.Equal(5752906, q.Id);
            Assert.Equal("Is reading the `length` property of an array really that expensive an operation in JavaScript?",q.Title);
        }

    }
}
