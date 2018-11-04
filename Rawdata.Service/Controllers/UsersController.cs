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
    public class UsersController : ControllerBase
    {
        protected readonly IMapper DtoMapper;
        protected readonly IUserService UserService;
        protected readonly ICommentService CommentService;

        public UsersController(IMapper dtoMapper, IUserService userService, ICommentService commentService)
        {
            DtoMapper = dtoMapper;
            UserService = userService;
            CommentService = commentService;
        }

        [Authorize, HttpGet("{id:int}", Name = "GetUserById")]
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

        [Authorize, HttpGet("{id:int}/comments", Name = "QueryMarkedComments")]
        public async Task<IActionResult> QueryMarkedComments([FromQuery] PagingDto paging, int id)
        {
            var result = await CommentService
                .QueryMarkedComments(id, paging.Search, paging.Page, paging.Size)
                .Include(c => c.Author)
                .ToListAsync();

            return Ok(
                DtoMapper.Map<IList<Comment>, IList<CommentDto>>(result)
            );
        }
    }
}
