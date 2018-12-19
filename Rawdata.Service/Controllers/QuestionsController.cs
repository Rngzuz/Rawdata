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

            var markedComments = await UserService
                .GetMarkedComments(GetUserId())
                .ToListAsync();

            //Check if the question is marked
            var markedPost = markedPosts
                .SingleOrDefault(mp => mp.PostId == id);

            var questionDto = DtoMapper.Map<QuestionDto>(result);
            questionDto.Marked = markedPost != null;

            if (markedPost != null)
            {
                questionDto.Note = markedPost.Note;
            }

            //Check if question comments are marked
            foreach (var commentDto in questionDto.Comments )
            {
                var markedComment = markedComments
                    .SingleOrDefault(mc => mc.CommentId == commentDto.Id);

                commentDto.Marked = markedComment != null;
                if (markedComment != null)
                {
                    commentDto.Note = markedComment.Note;
                }
            }


            // for each answer check if it is marked and if the answer's comments are marked
            foreach (var answerDto in questionDto.Answers )
            {
                var markedAnswer = markedPosts
                    .SingleOrDefault(mp => mp.PostId == answerDto.Id);

                answerDto.Marked = markedAnswer != null;
                if (markedAnswer != null)
                {
                    answerDto.Note = markedAnswer.Note;
                }

                foreach (var answerCommentDto in answerDto.Comments)
                {
                    var markedComment = markedComments
                        .SingleOrDefault(mc => mc.CommentId == answerCommentDto.Id);

                    answerCommentDto.Marked = markedComment != null;
                    if (markedComment != null)
                    {
                        answerCommentDto.Note = markedComment.Note;
                    }
                }
            }

            return Ok(
                questionDto
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

            foreach (var questionDto in dtos)
            {
                var markedPost = markedPosts
                    .SingleOrDefault(mp => mp.PostId == questionDto.Id);

                questionDto.Marked = markedPost != null;
                if (markedPost != null)
                {
                    questionDto.Note = markedPost.Note;
                }
            }

            return Ok(
                dtos
            );
        }
    }
}
