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
    [ApiController, Route("api/answers"), Produces("application/json")]
    public class AnswersController : BaseController
    {
        protected readonly IAnswerService AnswerService;

        public AnswersController(IMapper dtoMapper, IAnswerService answerService) : base(dtoMapper)
        {
            AnswerService = answerService;
        }

        [HttpGet("{id:int}", Name = GET_ANSWER_BY_ID)]
        public async Task<IActionResult> GetAnswerById(int id)
        {
            var result = await AnswerService.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(
                DtoMapper.Map<AnswerDto>(result)
            );
        }

        [HttpGet(Name = QUERY_ANSWERS)]
        public async Task<IActionResult> QueryAnswers([FromQuery] PagingDto paging)
        {
            var result = await AnswerService
                .QueryAnswers(GetUserId(), paging.Search, paging.Page, paging.Size)
                .Include(a => a.Author)
                .Include(a => a.Parent)
                .ToListAsync();

            return Ok(
                DtoMapper.Map<IList<Answer>, IList<AnswerDto>>(result)
            );
        }
    }
}