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
    }
}
