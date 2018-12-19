using Rawdata.Data;
using Rawdata.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;
using Rawdata.Service.Profiles;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Http;
using Rawdata.Data.Services.Interfaces;

namespace Rawdata.Tests
{
    public class Setup
    {
        protected readonly ITestOutputHelper Output;
        protected readonly DataContext Context;
        protected readonly IUrlHelper UrlHelper;
        protected readonly IMapper DtoMapper;
        protected readonly IAnswerService AnswerService;
        protected readonly IAuthorService AuthorService;
        protected readonly ICommentService CommentService;
        protected readonly IQuestionService QuestionService;
        protected readonly ISearchResultService SearchResultService;
        protected readonly IUserService UserService;

        public Setup(ITestOutputHelper output)
        {
            Output = output;
            Context = new DataContext();

            var moqUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            moqUrlHelper.Setup(
                urlHelper => urlHelper.Link(
                    It.IsAny<string>(),
                    It.IsAny<object>()
                )
            )
            .Returns("");

            UrlHelper = moqUrlHelper.Object;
            DtoMapper = CreateMapper(UrlHelper);

            AnswerService = new AnswerService(Context);
            AuthorService = new AuthorService(Context);
            CommentService = new CommentService(Context);
            QuestionService = new QuestionService(Context);
            SearchResultService = new SearchResultService(Context);
            UserService = new UserService(Context);
        }

        protected ControllerContext CreateControllerContext()
        {
            return new ControllerContext {
                HttpContext = new DefaultHttpContext()
            };
        }

        private IMapper CreateMapper(IUrlHelper url)
        {
            var mapperCfg = new MapperConfiguration(cfg => {
                cfg.AddProfile(new AnswerProfile(url));
                cfg.AddProfile(new AuthorProfile(url));
                cfg.AddProfile(new CommentProfile(url));
                cfg.AddProfile(new QuestionProfile(url));
                cfg.AddProfile(new UserProfile(url));
            });

            return mapperCfg.CreateMapper();
        }
    }
}
