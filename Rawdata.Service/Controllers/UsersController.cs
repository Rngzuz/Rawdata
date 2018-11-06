using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
    [ApiController, Route("api/users"), Produces("application/json")]
    public class UsersController : BaseController
    {
        protected readonly IUserService UserService;
        protected readonly ICommentService CommentService;
        protected readonly IQuestionService QuestionService;
        protected readonly IAnswerService AnswerService;

        public UsersController(IMapper dtoMapper, IUserService userService, ICommentService commentService, IQuestionService questionService, IAnswerService answerService) : base(dtoMapper)
        {
            UserService = userService;
            CommentService = commentService;
            QuestionService = questionService;
            AnswerService = answerService;
        }

        [Authorize, HttpGet("{id:int}", Name = GET_USER_BY_ID)]
        public async Task<IActionResult> GetUserById(int id)
        {
            UserDto user;

            try {
                var result = await UserService.GetUserById(id);
                user = DtoMapper.Map<UserDto>(result);
            }
            catch {
                return StatusCode(500);
            }

            if (user == null) {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize, HttpGet("comments", Name = QUERY_MARKED_COMMENTS)]
        public async Task<IActionResult> QueryMarkedComments([FromQuery] PagingDto paging)
        {
            var result = await CommentService
                .QueryMarkedComments(GetUserId(), paging.Search, paging.Page, paging.Size)
                .Include(c => c.Author)
                .ToListAsync();

            return Ok(
                DtoMapper.Map<IList<Comment>, IList<CommentDto>>(result)
            );
        }

        [Authorize, HttpPost("comments", Name = TOGGLE_MARKED_COMMENT)]
        public async Task<IActionResult> ToggleMarkedComment([FromBody] (int commentId, string note) body)
        {
            var result = await CommentService
                .ToggleMarkedComment(GetUserId(), body.commentId, body.note)
                .Include(c => c.Comment)
                    .ThenInclude(c => c.Author)
                .SingleOrDefaultAsync();

            return Ok(
                DtoMapper.Map<MarkedComment, MarkedCommentDto>(result)
            );
        }

        [Authorize, HttpGet("questions", Name = QUERY_MARKED_QUESTIONS)]
        public async Task<IActionResult> QueryMarkedQuestions([FromQuery] PagingDto paging, [FromQuery] string[] tags, [FromQuery] bool answeredOnly)
        {
            var result = await QuestionService
                .QueryMarkedQuestions(GetUserId(), paging.Search, tags, answeredOnly, paging.Page, paging.Size)
                .Include(c => c.Author)
                .ToListAsync();

            return Ok(
                DtoMapper.Map<IList<Question>, IList<QuestionDto>>(result)
            );
        }

        [Authorize, HttpGet("answers", Name = QUERY_MARKED_ANSWERS)]
        public async Task<IActionResult> QueryMarkedAnswers([FromQuery] PagingDto paging)
        {
            var result = await AnswerService
                .QueryMarkedAnswers(GetUserId(), paging.Search, paging.Page, paging.Size)
                .Include(c => c.Author)
                .Include(c => c.Parent)
                .ToListAsync();

            return Ok(
                DtoMapper.Map<IList<Answer>, IList<AnswerDto>>(result)
            );
        }
    }
}
