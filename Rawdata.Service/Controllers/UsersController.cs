using System;
using System.Collections.Generic;
using System.Dynamic;
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

        public UsersController(IMapper dtoMapper, IUserService userService, ICommentService commentService,
            IQuestionService questionService, IAnswerService answerService) : base(dtoMapper)
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

            try
            {
                var result = await UserService.GetUserById(id);
                user = DtoMapper.Map<UserDto>(result);
            }
            catch
            {
                return StatusCode(500);
            }

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize, HttpGet("profile", Name = GET_USER_PROFILE)]
        public async Task<IActionResult> GetUserProfile()
        {
            int? id = GetUserId();

            if (id == null)
            {
                return Unauthorized();
            }

            User user;
            dynamic userDto;

//            try {
            //TODO fix this 2 request hack
            user = await UserService.GetUserById(id);
            IList<MarkedPost> markedPosts = await UserService.GetMarkedPosts(id).ToListAsync();
            userDto = await MapUserToDto(user, markedPosts);
//            }
//            catch {
//                return StatusCode(500);
//            }

            return Ok(userDto);
        }


        [Authorize, HttpGet("", Name = GET_USER_BY_EMAIL)]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            UserDto user;

            try
            {
                var result = await UserService.GetUserByEmail(email);
                user = DtoMapper.Map<UserDto>(result);
            }
            catch
            {
                return StatusCode(500);
            }

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
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
        public async Task<IActionResult> UpdateMarkedComment([FromRoute] int commentId,
            [FromBody] ToggleCommentDto commentDto)
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


        [Authorize, HttpGet("{userId:int}/posts", Name = GET_MARKED_POSTS)]
        public async Task<IActionResult> GetMarkedPosts(int userId)
        {
            IList<MarkedPost> markedPosts = await UserService.GetMarkedPosts(userId).ToListAsync();

            ICollection<dynamic> markedPostsDto = await MapMarkedPostToDto(markedPosts);

            return Ok(
                markedPostsDto
            );
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

        protected async Task<dynamic> MapUserToDto(User user, ICollection<MarkedPost> markedPosts)
        {
            dynamic userDto = new ExpandoObject();

            userDto.DisplayName = user.DisplayName;
            userDto.Email = user.Email;
            userDto.CreationDate = user.CreationDate;
            userDto.SearchHistory = DtoMapper.Map<ICollection<Search>, ICollection<SearchDto>>(user.Searches);
            userDto.MarkedPosts = await MapMarkedPostToDto(markedPosts);
//            userDto.MarkedPosts = await MapMarkedPostToDto(user.MarkedPosts);
            userDto.MarkedComments =
                DtoMapper.Map<ICollection<MarkedComment>, ICollection<MarkedCommentDto>>(user.MarkedComments);
            ;
            userDto.Links = new
            {
                Self = Url.Link(GET_USER_BY_ID, new {Id = user.Id})
            };

            return userDto;
        }


        protected async Task<ICollection<dynamic>> MapMarkedPostToDto(ICollection<MarkedPost> result)
        {
            var items = new List<dynamic>();

            foreach (var item in result)
            {
                dynamic obj = new ExpandoObject();

                obj.body = item.Post.Body;
                obj.Score = item.Post.Score;
                obj.Score = item.Post.Score;
                obj.CreationDate = item.Post.CreationDate;
                obj.AuthorDisplayName = item.Post.Author.DisplayName;
                obj.Note = item.Note;

                if (item.Post is Question q)
                {
                    obj.Title = q.Title;

                    obj.Links = new
                    {
                        Self = Url.Link(GET_QUESTION_BY_ID, new {Id = q.Id}),
                        Author = Url.Link(GET_AUTHOR_BY_ID, new {Id = q.AuthorId})
                    };
                }
                else if (item.Post is Answer a)
                {
                    obj.Links = new
                    {
                        Self = Url.Link(GET_ANSWER_BY_ID, new {Id = a.Id}),
                        Parent = Url.Link(GET_QUESTION_BY_ID, new {Id = a.ParentId}),
                        Author = Url.Link(GET_AUTHOR_BY_ID, new {Id = a.AuthorId})
                    };
                }

                items.Add(obj);
            }

            return items;
        }
    }
}