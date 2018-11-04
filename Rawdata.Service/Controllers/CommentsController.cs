using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
    [ApiController]
    [Route("api/comments")]
    [Produces("application/json")]
    public class CommentsController : ControllerBase
    {
        protected readonly IMapper DtoMapper;
        protected readonly ICommentService Service;

        public CommentsController(IMapper dtoMapper, ICommentService service)
        {
            DtoMapper = dtoMapper;
            Service = service;
        }

        [HttpGet("{id:int}", Name = "GetCommentById")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            CommentDto comment;

            try {
                var result = await Service.GetCommentById(id);
                comment = DtoMapper.Map<CommentDto>(result);
            }
            catch {
                return StatusCode(500);
            }

            if (comment == null) {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpGet(Name = "QueryComments")]
        public async Task<IActionResult> QueryComments([FromQuery] PageQuery query)
        {
            IList<CommentDto> comments;

            try {
                var result = await Service
                    .QueryComments(null, query.Search, query.Page, query.Size)
                    .Include(c => c.Author)
                    .ToListAsync();

                comments = DtoMapper
                    .Map<IList<Comment>, IList<CommentDto>>(result);
            }
            catch {
                return StatusCode(500);
            }

            return Ok(comments);
        }

        // Add authorize attribute and move to UsersController
        [HttpGet("marked/{userId:int}", Name = "QueryMarkedComments")]
        public async Task<IActionResult> QueryMarkedComments([FromQuery] PageQuery query, int userId)
        {
            IList<CommentDto> comments;

            try {
                var result = await Service
                    .QueryMarkedComments(userId, query.Search, query.Page, query.Size)
                    .Include(c => c.Author)
                    .ToListAsync();

                comments = DtoMapper
                    .Map<IList<Comment>, IList<CommentDto>>(result);
            }
            catch {
                return StatusCode(500);
            }

            return Ok(comments);
        }
    }
}
