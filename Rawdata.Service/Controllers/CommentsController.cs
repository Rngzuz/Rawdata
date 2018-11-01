using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Repositories.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
    [ApiController]
    [Route("api/comments")]
    [Produces("applicaiton/json")]
    public class CommentsController : ControllerBase
    {
        protected readonly ICommentRepository Repository;

        public CommentsController(ICommentRepository repository)
        {
            Repository = repository;
        }

        [HttpGet("{userId:int?}")]
        public async Task<IActionResult> GetAll([FromQuery] PageQuery query, int? userId)
        {
            var result = await Repository
                .GetAll(userId, query.Search, query.Page, query.Size);

            // TODO: Failure logic and use DTOs

            return Ok(result);
        }

        // Add authorize attribute
        [HttpGet("marked/{userId:int}")]
        public async Task<IActionResult> GetAllMarked([FromQuery] PageQuery query, int userId)
        {
            var result = await Repository
                .GetAllMarked(userId, query.Search, query.Page, query.Size);

            // TODO: Failure logic and use DTOs

            return Ok(result);
        }
    }
}
