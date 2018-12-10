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

        [HttpGet(Name = GET_NEWEST_QUESTIONS)]
        public async Task<IActionResult> GetNewestQuestions([FromQuery] Paging paging)
        {
            var result = await QuestionService.GetNewestQuestions(paging.Page, paging.Size).ToListAsync();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(
                DtoMapper.Map<IList<Question>, IList<QuestionDto>>(result)
            );
        }
    }
}
