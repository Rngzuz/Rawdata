using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
    [ApiController, Route("api/search"), Produces("application/json")]
    public class SearchController : BaseController
    {
        protected readonly ISearchResultService SearchResultService;
        protected readonly IUserService UserService;

        public SearchController(IMapper dtoMapper, ISearchResultService searchResultService, IUserService userService) : base(dtoMapper)
        {
            SearchResultService = searchResultService;
            UserService = userService;
        }

        [HttpGet("exact", Name = "GetExactMatch")]
        public async Task<IActionResult> GetExactMatch([FromQuery]Paging paging)
        {
            var result = await SearchResultService
               .GetExactMatch(paging.Page, paging.Size, paging.Words)
               .ToListAsync();

            await UserService.SaveToSearchHistory(GetUserId(), string.Join(" ", paging.Words));

            return Ok(await DirtyMap(result));
        }

        [HttpGet("best", Name = "GetBestMatch")]
        public async Task<IActionResult> GetBestMatch([FromQuery]Paging paging)
        {
            var result = await SearchResultService
                .GetBestMatch(paging.Page, paging.Size, paging.Words)
                .ToListAsync();

            await UserService.SaveToSearchHistory(GetUserId(), string.Join(" ", paging.Words));

            return Ok(await DirtyMap(result));
        }

        [HttpGet("ranked", Name = "GetRankedWeightedMatch")]
        public async Task<IActionResult> GetRankedWeightedMatch([FromQuery]Paging paging)
        {
            var result = await SearchResultService
                .GetRankedWeightedMatch(paging.Page, paging.Size, paging.Words)
                .ToListAsync();

            await UserService.SaveToSearchHistory(GetUserId(), string.Join(" ", paging.Words));

            return Ok(await DirtyMap(result));
        }

        [HttpGet("words", Name = "GetWords")]
        public async Task<IActionResult> GetWords([FromQuery] string word, [FromQuery] int size = 100)
        {
            var result = await SearchResultService
                .GetWeightedKeywords(size, word)
                .ToListAsync();

            await UserService.SaveToSearchHistory(GetUserId(), word);

            return Ok(result);
        }

        [HttpGet("context", Name = "GetContext")]
        public async Task<IActionResult> GetContext([FromQuery] string word, [FromQuery] int size = 100)
        {
            var result = await SearchResultService
                .GetWordAssociation(size, word)
                .ToListAsync();

            await UserService.SaveToSearchHistory(GetUserId(), word);

            return Ok(result);
        }

        //TODO: We want to find a better approach to map
        protected async Task<IList<dynamic>> DirtyMap(IList<SearchResult> result)
        {
            var items = new List<dynamic>();
            var markedPosts = await UserService
                .GetMarkedPosts(GetUserId())
                .ToListAsync();

            foreach (var item in result) {
                dynamic obj = new ExpandoObject();
                var markedPost = markedPosts
                    .SingleOrDefault(mp => mp.PostId == item.PostId);

                obj.Body = item.Post.Body;
                obj.Score = item.Post.Score;
                obj.Rank = item.Rank;
                obj.CreationDate = item.Post.CreationDate;
                obj.AuthorDisplayName = item.Post.Author.DisplayName;
                obj.Marked = markedPost != null;

                if (obj.Marked) {
                    obj.Note = markedPost.Note;
                }

                if (item.Post is Question q) {
                    obj.Title = q.Title;

                    obj.Links = new
                    {
                        Self = Url.Link(GET_QUESTION_BY_ID, new { Id = q.Id }),
                        Author = Url.Link(GET_AUTHOR_BY_ID, new { Id = q.AuthorId })
                    };
                }
                else if (item.Post is Answer a) {
                    obj.Links = new
                    {
                        Self = Url.Link(GET_ANSWER_BY_ID, new { Id = a.Id }),
                        Parent = Url.Link(GET_QUESTION_BY_ID, new { Id = a.ParentId }),
                        Author = Url.Link(GET_AUTHOR_BY_ID, new { Id = a.AuthorId })
                    };
                }

                items.Add(obj);
            }

            return items;
        }
    }
}
