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
    [ApiController, Route("api/questions"), Produces("application/json")]
    public class QuestionsController : BaseController
    {
        protected readonly IQuestionService QuestionService;

        public QuestionsController(IMapper dtoMapper, IQuestionService questionService) : base(dtoMapper)
        {
            QuestionService = questionService;
        }

        [HttpGet("{id:int}", Name = GET_QUESTION_BY_ID)]
        public async Task<IActionResult> GetQuestionById(int id)
        {
            var result = await QuestionService.GetQuestionById(id);

            if (result == null) {
                return NotFound();
            }

            return Ok(
                DtoMapper.Map<QuestionDto>(result)
            );
        }

        [HttpGet(Name = QUERY_QUESTIONS)]
        public async Task<IActionResult> QueryQuestions([FromQuery] PagingDto paging, [FromQuery] string[] tags, [FromQuery] bool answeredOnly)
        {
            var result = await QuestionService
                .QueryQuestions(GetUserId(), paging.Search, tags, answeredOnly, paging.Page, paging.Size)
                .Include(q => q.Author)
                .ToListAsync();

            return Ok(
                DtoMapper.Map<IList<Question>, IList<QuestionDto>>(result)
            );
        }
    }
}
