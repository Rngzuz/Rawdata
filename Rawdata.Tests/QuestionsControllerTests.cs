using Xunit;
using Rawdata.Service.Models;
using Rawdata.Service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Rawdata.Service.Profiles;
using System.Threading.Tasks;
using Xunit.Abstractions;
using System.Collections.Generic;

namespace Rawdata.Tests
{
    public class QuestionsControllerTests : Setup
    {
        private readonly QuestionsController Controller;

        public QuestionsControllerTests(ITestOutputHelper output) : base(output)
        {
            Controller = new QuestionsController(DtoMapper, QuestionService, UserService) {
                Url = UrlHelper,
                ControllerContext = CreateControllerContext()
            };
        }

        [Theory]
        [InlineData(388242)]
        [InlineData(231767)]
        [InlineData(9033)]
        public async Task GetQuestionById_Should_Succeed(int questionId)
        {
            var result = await Controller.GetQuestionById(questionId) as ObjectResult;

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<QuestionDto>(result.Value);
        }

        [Fact]
        public async Task GetQuestionById_Should_Fail()
        {
            var result = await Controller.GetQuestionById(0);
            var statusCode = (int)result.GetType().GetProperty("StatusCode").GetValue(result, null);

            Assert.Equal(404, statusCode);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(1, 50)]
        [InlineData(2, 100)]
        public async Task GetNewestQuestions_Should_Succeed(int page, int size)
        {
            var paging = new Paging { Page = page, Size = size };
            var result = await Controller.GetNewestQuestions(paging) as OkObjectResult;

            Assert.IsType<List<QuestionDto>>(result.Value);
            Assert.Equal(size, (result.Value as List<QuestionDto>).Count);
        }

        [Fact]
        public async Task GetNewestQuestions_Should_Be_Empty()
        {
            var paging = new Paging { Page = 10000, Size = 100 };
            var result = await Controller.GetNewestQuestions(paging) as OkObjectResult;

            Assert.IsType<List<QuestionDto>>(result.Value);
            Assert.Empty((result.Value as List<QuestionDto>));
        }
    }
}
