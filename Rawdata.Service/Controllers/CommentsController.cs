using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
    [ApiController]
    [Route("api/comments")]
    [Produces("application/json")]
    public class CommentsController : ControllerBase
    {
        protected readonly ICommentService Service;

        public CommentsController(ICommentService service)
        {
            Service = service;
        }

        [HttpGet("{userId:int?}")]
        public async Task<IActionResult> GetAll([FromQuery] PageQuery query, int? userId)
        {
            var result = await Service
                .QueryComments(userId, query.Search, query.Page, query.Size)
                .ToListAsync();

            // TODO: Failure logic and use DTOs

            return Ok(result);
        }

        // Add authorize attribute
        [HttpGet("marked/{userId:int}")]
        public async Task<IActionResult> GetAllMarked([FromQuery] PageQuery query, int userId)
        {
            var result = await Service
                .QueryMarkedComments(userId, query.Search, query.Page, query.Size)
                .ToListAsync();

            // TODO: Failure logic and use DTOs

            return Ok(result);
        }
    }
}
