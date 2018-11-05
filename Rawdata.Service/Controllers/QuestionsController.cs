using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
            throw new NotImplementedException();
        }

        [HttpGet(Name = "QueryQuestions")]
        public async Task<IActionResult> QueryQuestions([FromBody] PagingDto paging)
        {
            throw new NotImplementedException();
        }
    }
}
