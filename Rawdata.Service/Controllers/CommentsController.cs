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
    [ApiController, Route("api/comments"), Produces("application/json")]
    public class CommentsController : BaseController
    {
        protected readonly ICommentService Service;

        public CommentsController(IMapper dtoMapper, ICommentService service) : base(dtoMapper)
        {
            Service = service;
        }

        [HttpGet("{id:int}", Name = GET_COMMENT_BY_ID)]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var result = await Service.GetCommentById(id);

            if (result == null) {
                return NotFound();
            }

            return Ok(
                DtoMapper.Map<CommentDto>(result)
            );
        }

        [HttpGet(Name = QUERY_COMMENTS)]
        public async Task<IActionResult> QueryComments([FromQuery] PagingDto paging)
        {
            var result = await Service
                .QueryComments(null, paging.Search, paging.Page, paging.Size)
                .Include(c => c.Author)
                .ToListAsync();

            return Ok(
                DtoMapper.Map<IList<Comment>, IList<CommentDto>>(result)
            );
        }
    }
}
