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
            var result = await AnswerService.GetAnswerById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(
                DtoMapper.Map<AnswerDto>(result)
            );
        }
    }
}
