using System;
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
    [Route("api/questions")]
    [Produces("application/json")]
    public class QuestionsController : ControllerBase
    {
        protected readonly IMapper DtoMapper;
        protected readonly IQuestionService QuestionService;

        public QuestionsController(IMapper dtoMapper, IQuestionService questionService)
        {
            DtoMapper = dtoMapper;
            QuestionService = questionService;
        }

        [HttpGet("{id:int}", Name = "GetQuestionById")]
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

        [HttpGet(Name = "QueryQuestions")]
        public async Task<IActionResult> QueryQuestions([FromQuery] PagingDto paging, [FromQuery] string[] tags, [FromQuery] bool answeredOnly)
        {
            var result = await QuestionService
                .QueryQuestions(GetAuthorizedUserId(), paging.Search, tags, answeredOnly, paging.Page, paging.Size)
                .Include(q => q.Author)
                .Include(q => q.Comments)
                .Include(q => q.Answers)
                    .ThenInclude(a => a.Comments)
                .ToListAsync();

            return Ok(
                DtoMapper.Map<IList<Question>, IList<QuestionDto>>(result)
            );
        }

        public int? GetAuthorizedUserId()
        {
            var claim = User.FindFirst("id")?.Value;

            if (int.TryParse(claim, out int userId)) {
                return userId;
            }

            return null;
        }
    }
}
