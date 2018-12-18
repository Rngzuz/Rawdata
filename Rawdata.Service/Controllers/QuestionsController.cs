using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
    [ApiController, Route("api/questions"), Produces("application/json")]
    public class QuestionsController : BaseController
    {
        protected readonly IQuestionService QuestionService;
        protected readonly IUserService UserService;

        public QuestionsController(IMapper dtoMapper, IQuestionService questionService, IUserService userService) : base(dtoMapper)
        {
            QuestionService = questionService;
            UserService = userService;
        }

        [HttpGet("{id:int}", Name = GET_QUESTION_BY_ID)]
        public async Task<IActionResult> GetQuestionById(int id)
        {
            var result = await QuestionService.GetQuestionById(id);

            if (result == null) {
                return NotFound();
            }
            
            var markedPosts = await UserService
                .GetMarkedPosts(GetUserId())
                .ToListAsync();
            
            var markedPost = markedPosts
                .SingleOrDefault(mp => mp.PostId == id);

            var dto = DtoMapper.Map<QuestionDto>(result);
            dto.Marked = markedPost != null;
            
            return Ok(
                dto
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

            var markedPosts = await UserService
                .GetMarkedPosts(GetUserId())
                .ToListAsync();
            
            var dtos = DtoMapper.Map<IList<Question>, IList<QuestionDto>>(result);

            foreach (var dto in dtos)
            {
                var markedPost = markedPosts
                    .SingleOrDefault(mp => mp.PostId == dto.Id);
                
                dto.Marked = markedPost != null;
            }

            return Ok(
                dtos
            );
        }
    }
}
