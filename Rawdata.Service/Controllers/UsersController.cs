using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        [Authorize, HttpGet("questions", Name = "GetQuestionsWithMarkedPosts")]
        public async Task<IActionResult> GetQuestionsWithMarkedPosts([FromQuery] PagingDto paging)
        {
            var result = await QuestionService
                .GetQuestionsWithMarkedPosts(GetUserId(), paging.Page, paging.Size);

            return Ok(
                DtoMapper.Map<IList<Question>, IList<QuestionListDto>>(result)
            );
        }

        [Authorize, HttpGet("comments", Name = "GetQuestionsWithMarkedComments")]
        public async Task<IActionResult> GetQuestionsWithMarkedComments([FromQuery] PagingDto paging)
        {
            var result = await QuestionService
                .GetQuestionsWithMarkedComments(GetUserId(), paging.Page, paging.Size);

            return Ok(
                DtoMapper.Map<IList<Question>, IList<QuestionListDto>>(result)
            );
        }

        [Authorize, HttpGet("{userId:int}/history", Name = GET_USER_HISTORY)]
        public async Task<IActionResult> GetUserHistory(int userId)
        {
            IList<Search> searches = await UserService.GetUserHistory(userId);

            return Ok(
                    DtoMapper.Map<IList<Search>, IList<SearchDto>>(searches)
           );
        }

        [Consumes("application/json")]
        [Authorize, HttpPost("comments", Name = TOGGLE_MARKED_COMMENT)]
        public async Task<IActionResult> ToggleMarkedComment([FromBody] ToggleCommentDto commentDto)
        {
            var result = await CommentService
                .ToggleMarkedComment(GetUserId(), commentDto.CommentId, commentDto.Note)
                .Include(c => c.Comment)
                    .ThenInclude(c => c.Author)
                .SingleOrDefaultAsync();

            return Ok(
                DtoMapper.Map<MarkedComment, MarkedCommentDto>(result)
            );
        }

        [Consumes("application/json")]
        [Authorize, HttpPost("comments/{commentId:int}", Name = UPDATE_MARKED_COMMENT)]
        public async Task<IActionResult> UpdateMarkedComment([FromRoute] int commentId, [FromBody] ToggleCommentDto commentDto)
        {
            var result = await CommentService
                .UpdateMarkedCommentNote(GetUserId(), commentId, commentDto.Note);

            return result ? StatusCode(204) : NotFound();
        }

        [Consumes("application/json")]
        [Authorize, HttpPost("posts/{postId:int}", Name = UPDATE_MARKED_POST)]
        public async Task<IActionResult> UpdateMarkedPost([FromRoute] int postId, [FromBody] TogglePostDto postDto)
        {
            var result = await UserService
                .UpdateMarkedPostNote(GetUserId(), postId, postDto.Note);

            return result ? StatusCode(204) : NotFound();
        }

        [Consumes("application/json")]
        [Authorize, HttpPost("posts", Name = TOGGLE_MARKED_POST)]
        public async Task<IActionResult> ToggleMarkedPost([FromBody] TogglePostDto postDto)
        {
            var result = await UserService
                .ToggleMarkedPost(GetUserId(), postDto.PostId, postDto.Note)
                .Include(p => p.Post)
                    .ThenInclude(p => p.Author)
                .SingleOrDefaultAsync();

            //If nothing is returned - post is unmarked -> return empty
            if (result == null)
            {
                return StatusCode(204);
            }

            //Else check if question or not and return appropriate DTO
            var question = await QuestionService.GetQuestionById(result.PostId);
            if (question != null)
            {
                return Ok(
                        DtoMapper.Map<MarkedPost, MarkedQuestionDto>(result)
                    );
            }
            else
            {
                return Ok(
                    DtoMapper.Map<MarkedPost, MarkedAnswerDto>(result)
                );
            }

        }
    }
}
